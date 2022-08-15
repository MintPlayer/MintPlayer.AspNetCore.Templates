using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Mappers;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;
using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Enums;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Repositories;

internal class AccountRepository : IAccountRepository
{
	#region Constructor
	private readonly UserManager<Persistance.Entities.User> userManager;
	private readonly SignInManager<Persistance.Entities.User> signInManager;
	private readonly IUserMapper userMapper;
	private readonly IHttpContextAccessor httpContextAccessor;
	public AccountRepository(
		UserManager<Persistance.Entities.User> userManager,
		SignInManager<Persistance.Entities.User> signInManager,
		IUserMapper userMapper,
		IHttpContextAccessor httpContextAccessor)
	{
		this.userManager = userManager;
		this.signInManager = signInManager;
		this.userMapper = userMapper;
		this.httpContextAccessor = httpContextAccessor;
	}
	#endregion

	public async Task<User> GetCurrentUser()
	{
		var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext!.User);
		var dto = await userMapper.Entity2Dto(user);
		return dto;
	}

	public async Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders()
	{
		var providers = await signInManager.GetExternalAuthenticationSchemesAsync();
		return providers;
	}

	public Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
	{
		var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return Task.FromResult(properties);
	}

	public async Task<ExternalLoginResult> PerformExternalLogin()
	{
		var info = await signInManager.GetExternalLoginInfoAsync();
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

		var signinResult = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true, user.Bypass2faForExternalLogin);
		if (signinResult.Succeeded)
		{
			return new ExternalLoginResult
			{
				Status = ELoginStatus.Success,
				Provider = info.LoginProvider,
				User = await userMapper.Entity2Dto(user),
			};
		}
		else if (signinResult.RequiresTwoFactor)
		{
			return new ExternalLoginResult
			{
				Status = ELoginStatus.RequiresTwoFactor,
				Provider = info.LoginProvider,
				User = await userMapper.Entity2Dto(user),
			};
		}
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

	public async Task Logout()
	{
		await signInManager.SignOutAsync();
	}
}
