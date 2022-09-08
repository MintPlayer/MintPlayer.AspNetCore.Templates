using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using MintPlayer.AspNetCore.MustChangePassword.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Mappers;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Enums;
using System.Security.Claims;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Repositories;

internal class AccountRepository : IAccountRepository
{
	#region Constructor
	private readonly UserManager<Persistance.Entities.User> userManager;
	private readonly SignInManager<Persistance.Entities.User> signinManager;
	private readonly SsoContext ssoContext;
	private readonly IOptions<IdentityOptions> identityOptions;
	private readonly IUserMapper userMapper;
	private readonly IHttpContextAccessor httpContextAccessor;
	private readonly IServiceProvider serviceProvider;
	private readonly IMustChangePasswordService<Persistance.Entities.User, Guid> mustChangePasswordService;
	public AccountRepository(
		UserManager<Persistance.Entities.User> userManager,
		SignInManager<Persistance.Entities.User> signinManager,
		SsoContext ssoContext,
		IOptions<IdentityOptions> identityOptions,
		IUserMapper userMapper,
		IHttpContextAccessor httpContextAccessor,
		IServiceProvider serviceProvider,
		IMustChangePasswordService<Persistance.Entities.User, Guid> mustChangePasswordService)
	{
		this.userManager = userManager;
		this.signinManager = signinManager;
		this.ssoContext = ssoContext;
		this.identityOptions = identityOptions;
		this.userMapper = userMapper;
		this.httpContextAccessor = httpContextAccessor;
		this.serviceProvider = serviceProvider;
		this.mustChangePasswordService = mustChangePasswordService;
	}
	#endregion

	public async Task<User> Register(User user, string password)
	{
		var entity = await userMapper.Dto2Entity(user);
		await userManager.CreateAsync(entity, password);
		await userManager.AddClaimsAsync(entity, new[]
		{
			new System.Security.Claims.Claim("email", user.Email),
			new System.Security.Claims.Claim("name", user.UserName),
		});

		var dto = await userMapper.Entity2Dto(entity);
		return dto;
	}

#if (UseEmailConfirmation)
	public async Task<string> GenerateEmailConfirmationToken(string email)
	{
		var user = await userManager.FindByEmailAsync(email);
		var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
		return token;
	}

	public async Task VerifyEmailConfirmationToken(string email, string token)
	{
		var user = await userManager.FindByEmailAsync(email);
		var result = await userManager.ConfirmEmailAsync(user, token);
		if (!result.Succeeded)
		{
			throw new VerifyEmailException(
				new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)))
			);
		}
	}
#endif

	public async Task<User> Login(string email, string password, bool createCookie)
	{
		try
		{
			var user = await signinManager.UserManager.FindByEmailAsync(email);
			if (user == null) throw new LoginException { Email = email };

			if (createCookie)
			{
				var isPasswordCorrect = await userManager.CheckPasswordAsync(user, password);
				if (!isPasswordCorrect) throw new InvalidPasswordException();

#if (UseEmailConfirmation)
				var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
				if (identityOptions.Value.SignIn.RequireConfirmedEmail && !isEmailConfirmed) throw new EmailNotConfirmedException();

#endif
				var userDto = await userMapper.Entity2Dto(user);

				// 1) Password == "admin"
				// 2) Password correct
				// 3) Email = "admin@example.com"
				if ((password == "admin") && isPasswordCorrect && (email == "admin@example.com"))
				{
					// The following call will always throw MustChangePasswordException()
					await mustChangePasswordService.ChangePasswordSignInAsync(user, password);
				}

				var signinResult = await signinManager.PasswordSignInAsync(user, password, true, true);
				if (signinResult.Succeeded)
				{
					return userDto;
				}
#if (UseTwoFactorAuthentication)
				else if (signinResult.RequiresTwoFactor)
				{
					throw new RequiresTwoFactorException
					{
						User = userDto,
					};
				}
#endif
				else
				{
					throw new LoginException();
				}
			}
			else
			{
				var signinResult = await signinManager.CheckPasswordSignInAsync(user, password, true);
				if (!signinResult.Succeeded) throw new LoginException { UserId = user.Id, Email = email, Username = user.UserName };

#if (UseEmailConfirmation)
				var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
				if (!isEmailConfirmed) throw new EmailNotConfirmedException();

#endif
				if (signinResult.Succeeded)
				{
					var dto = await userMapper.Entity2Dto(user);
					return dto;
				}
#if (UseTwoFactorAuthentication)
				else if (signinResult.RequiresTwoFactor)
				{
					throw new RequiresTwoFactorException
					{
						User = await userMapper.Entity2Dto(user)
					};
				}
#endif
				else
				{
					throw new LoginException();
				}
			}
		}
		catch (LoginException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

#if (UseTwoFactorAuthentication)
	public async Task<User> TwoFactorLogin(string verificationCode, bool rememberDevice)
	{
		var result = await signinManager.TwoFactorAuthenticatorSignInAsync(verificationCode, true, rememberDevice);
		if (result.Succeeded)
		{
			var user = await signinManager.GetTwoFactorAuthenticationUserAsync();
			var dto = await userMapper.Entity2Dto(user);
			return dto;
		}
		else
		{
			throw new LoginException();
		}
	}

#endif
	public async Task<User> GetCurrentUser()
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
		var dto = await userMapper.Entity2Dto(user);
		return dto;
	}

	public async Task<IEnumerable<string>> GetRoles()
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
		var roles = await userManager.GetRolesAsync(user);
		return roles;
	}

	public async Task Logout()
	{
		await signinManager.SignOutAsync();
	}

	public async Task<bool> HasPassword()
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
		if (user == null) throw new UnauthorizedAccessException();

		var hasPassword = await userManager.HasPasswordAsync(user);
		return hasPassword;
	}

	public async Task ChangePassword(string? currentPassword, string newPassword, string newPasswordConfirmation)
	{
		if (newPassword != newPasswordConfirmation)
		{
			throw new PasswordConfirmationException();
		}

		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
		if (user == null) throw new UnauthorizedAccessException();

		var hasPassword = await userManager.HasPasswordAsync(user);
		IdentityResult result;
		if (!hasPassword && string.IsNullOrEmpty(currentPassword))
		{
			result = await userManager.AddPasswordAsync(user, newPassword);
		}
		else
		{
			result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
		}

		if (!result.Succeeded)
		{
			throw new InvalidPasswordException();
		}
	}

#if (UseTwoFactorAuthentication)
	public async Task<string> GenerateTwoFactorRegistrationCode()
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
		var code = await userManager.GetAuthenticatorKeyAsync(user);
		if (string.IsNullOrEmpty(code))
		{
			await userManager.ResetAuthenticatorKeyAsync(user);
			code = await userManager.GetAuthenticatorKeyAsync(user);
		}
		return code;
	}

	private async Task<bool> CheckTwoFactorCode(Persistance.Entities.User user, TwoFactorCode code)
	{
		switch (code.Type)
		{
			case ECodeType.VerificationCode:
				var isCodeCorrect = await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, code.Code);
				return isCodeCorrect;
			case ECodeType.RecoveryCode:
				var res = await userManager.RedeemTwoFactorRecoveryCodeAsync(user, code.Code);
				return res.Succeeded;
			default:
				throw new Exception("Invalid code type");
		}
	}

	public async Task SetEnableTwoFactor(bool enable, TwoFactorCode code)
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
		var isCodeCorrect = await CheckTwoFactorCode(user, code);

		if (!isCodeCorrect)
		{
			throw new UnauthorizedAccessException();
		}

		var result = await userManager.SetTwoFactorEnabledAsync(user, enable);
		if (!result.Succeeded)
		{
			throw new UnauthorizedAccessException();
		}
	}

	public async Task<int> GetRemainingRecoveryCodes()
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
		var count = await userManager.CountRecoveryCodesAsync(user);
		return count;
	}

	public async Task SetBypassTwoFactor(bool bypass, string verificationCode)
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);

		var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
		if (!is2faTokenValid)
		{
			throw new UnauthorizedAccessException();
		}

		user.Bypass2faForExternalLogin = bypass;
		await ssoContext.SaveChangesAsync();
	}

	public async Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodes(string verificationCode)
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);

		var is2faTokenValid = await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
		if (!is2faTokenValid)
		{
			throw new UnauthorizedAccessException();
		}

		var recoveryCodes = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
		return recoveryCodes;
	}

	public async Task<User> TwoFactorRecovery(string recoveryCode)
	{
		var result = await signinManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);
		if (result.Succeeded)
		{
			var user = await signinManager.GetTwoFactorAuthenticationUserAsync();
			var dto = await userMapper.Entity2Dto(user);
			return dto;
		}
		else
		{
			throw new LoginException();
		}
	}

#endif
#if (UseExternalLogins)
	public async Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders()
	{
		var providers = await signinManager.GetExternalAuthenticationSchemesAsync();
		return providers;
	}

	public Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
	{
		var properties = signinManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return Task.FromResult(properties);
	}

	public async Task<ExternalLoginResult> PerformExternalLogin()
	{
		var info = await signinManager.GetExternalLoginInfoAsync();
		if (info == null)
		{
			throw new UnauthorizedAccessException();
		}

		// Check if the login specified is already known by us
		var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
		if (user == null)
		{
			// User doesn't exist yet in our database. Create it.
			var username = info.Principal.FindFirstValue(ClaimTypes.Name);
			var email = info.Principal.FindFirstValue(ClaimTypes.Email);

			var newUser = new Persistance.Entities.User
			{
				UserName = username.Replace(" ", string.Empty),
				Email = email,
				EmailConfirmed = true,
			};

			var idResult = await userManager.CreateAsync(newUser);
			if (idResult.Succeeded)
			{
				user = newUser;
			}
			else
			{
				// User creation failed, probably because the email address is already present in the database
				if (idResult.Errors.Any(e => e.Code == "DuplicateEmail"))
				{
					var existing = await userManager.FindByEmailAsync(email);
					var existingLogins = await userManager.GetLoginsAsync(existing);

					if (existingLogins.Any())
					{
						throw new OtherAccountException(existingLogins);
					}
					else
					{
						throw new Exception("Could not create account from social profile");
					}
				}
				else
				{
					throw new Exception("Could not create account from social profile");
				}
			}
			await userManager.AddLoginAsync(user, new UserLoginInfo(info.LoginProvider, info.ProviderKey, info.ProviderDisplayName));
		}

		var signinResult = await signinManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true, user.Bypass2faForExternalLogin);
		if (signinResult.Succeeded)
		{
			return new ExternalLoginResult
			{
				Status = ELoginStatus.Success,
				Provider = info.LoginProvider,
				User = await userMapper.Entity2Dto(user),
			};
		}
#if (UseTwoFactorAuthentication)
		else if (signinResult.RequiresTwoFactor)
		{
			return new ExternalLoginResult
			{
				Status = ELoginStatus.RequiresTwoFactor,
				Provider = info.LoginProvider,
				User = await userMapper.Entity2Dto(user),
			};
		}
#endif
		else
		{
			return new ExternalLoginResult
			{
				Status = ELoginStatus.Failed,
				Error = "External login failed",
				ErrorDescription = $"Something went wrong while signing in with {info.LoginProvider}",
			};
		}
	}

	public async Task<IEnumerable<UserLoginInfo>> GetExternalLogins()
	{
		// Get current user
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
		if (user == null) throw new UnauthorizedAccessException();

		var user_logins = await userManager.GetLoginsAsync(user);
		return user_logins;
	}

	public async Task AddExternalLogin()
	{
		// Get current user
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
		if (user == null) throw new UnauthorizedAccessException();

		// Get login info
		var info = await signinManager.GetExternalLoginInfoAsync();
		if (info == null) throw new UnauthorizedAccessException();

		var result = await userManager.AddLoginAsync(user, info);
		if (!result.Succeeded) throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
	}

	public async Task RemoveExternalLogin(string provider)
	{
		// Get current user
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
		if (user == null) throw new UnauthorizedAccessException();

		var user_logins = await userManager.GetLoginsAsync(user);
		var login = user_logins.FirstOrDefault(l => l.LoginProvider == provider);

		if (login == null) throw new InvalidOperationException($"Could not remove {provider} login");

		var result = await userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
		if (!result.Succeeded) throw new Exception($"Could not remove {provider} login");
	}
#endif
}
