using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using MintPlayer.AspNetCore.MustChangePassword.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Options;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;
using System.Net.Mail;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

internal class AccountService : IAccountService
{
	#region Constructor
	private readonly IAccountRepository accountRepository;
	private readonly IMustChangePasswordService<Persistance.Entities.User, Guid> mustChangePasswordService;
	private readonly LinkGenerator linkGenerator;
	private readonly IHttpContextAccessor httpContextAccessor;
	private readonly IMailService mailService;
	private readonly IOptions<SmtpOptions> smtpOptions;
	private readonly IOptions<IdentityOptions> identityOptions;
	public AccountService(
		IAccountRepository accountRepository,
		IMustChangePasswordService<Persistance.Entities.User, Guid> mustChangePasswordService,
		LinkGenerator linkGenerator,
		IHttpContextAccessor httpContextAccessor,
		IMailService mailService,
		IOptions<Options.SmtpOptions> smtpOptions,
		IOptions<IdentityOptions> identityOptions)
	{
		this.accountRepository = accountRepository;
		this.mustChangePasswordService = mustChangePasswordService;
		this.linkGenerator = linkGenerator;
		this.httpContextAccessor = httpContextAccessor;
		this.mailService = mailService;
		this.smtpOptions = smtpOptions;
		this.identityOptions = identityOptions;
	}
	#endregion

	public async Task<RegisterResult> Register(User user, string password, string passwordConfirmation)
	{
		if (password != passwordConfirmation)
		{
			throw new PasswordConfirmationException();
		}

		var newUser = await accountRepository.Register(user, password);
		return new RegisterResult { User = newUser, RequiresEmailConfirmation = identityOptions.Value.SignIn.RequireConfirmedEmail };
	}

	public async Task<string> GenerateEmailConfirmationToken(string email)
	{
		var token = await accountRepository.GenerateEmailConfirmationToken(email);
		return token;
	}

	public async Task SendConfirmationEmail(string email, string confirmationUrl)
	{
		if (identityOptions.Value.SignIn.RequireConfirmedEmail)
		{
			var html = $@"Please confirm your account by clicking <a href=""{confirmationUrl}"">here</a>.";
			using (var client = await mailService.CreateSmtpClient())
			using (var message = new MailMessage(smtpOptions.Value.DefaultFrom, email, "Confirm email address", html))
			{
				message.IsBodyHtml = true;
				client.Send(message);
			}
		}
	}

	public async Task VerifyEmailConfirmationToken(string email, string token)
	{
		await accountRepository.VerifyEmailConfirmationToken(email, token);
	}


	public async Task<User> Login(string email, string password, bool createCookie)
	{
		var user = await accountRepository.Login(email, password, createCookie);
		return user;
	}

	public async Task<User> TwoFactorLogin(string verificationCode, bool rememberDevice)
	{
		var result = await accountRepository.TwoFactorLogin(verificationCode, rememberDevice);
		return result;
	}

	public async Task<User> GetCurrentUser()
	{
		var user = await accountRepository.GetCurrentUser();
		return user;
	}

	public async Task<IEnumerable<string>> GetRoles()
	{
		var roles = await accountRepository.GetRoles();
		return roles;
	}

	public async Task Logout()
	{
		await accountRepository.Logout();
	}

	public async Task<bool> HasPassword()
	{
		var hasPassword = await accountRepository.HasPassword();
		return hasPassword;
	}

	public async Task PerformMustChangePassword(string newPassword, string newPasswordConfirmation)
	{
		await mustChangePasswordService.PerformChangePasswordAsync(newPassword, newPasswordConfirmation);
	}

	public async Task ChangePassword(string? currentPassword, string newPassword, string newPasswordConfirmation)
	{
		await accountRepository.ChangePassword(currentPassword, newPassword, newPasswordConfirmation);
	}

	public async Task<string> GenerateTwoFactorRegistrationCode()
	{
		var code = await accountRepository.GenerateTwoFactorRegistrationCode();
		return code;
	}

	public async Task SetEnableTwoFactor(bool enable, TwoFactorCode code)
	{
		await accountRepository.SetEnableTwoFactor(enable, code);
	}

	public async Task<int> GetRemainingRecoveryCodes()
	{
		var count = await accountRepository.GetRemainingRecoveryCodes();
		return count;
	}

	public async Task SetBypassTwoFactor(bool bypass, string verificationCode)
	{
		await accountRepository.SetBypassTwoFactor(bypass, verificationCode);
	}

	public async Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodes(string verificationCode)
	{
		var recoveryCodes = await accountRepository.GenerateNewTwoFactorRecoveryCodes(verificationCode);
		return recoveryCodes;
	}

	public async Task<User> TwoFactorRecovery(string recoveryCode)
	{
		var user = await accountRepository.TwoFactorRecovery(recoveryCode);
		return user;
	}

	public async Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders()
	{
		var providers = await accountRepository.GetExternalLoginProviders();
		return providers;
	}

	public async Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
	{
		var properties = await accountRepository.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return properties;
	}

	public async Task<ExternalLoginResult> PerformExternalLogin()
	{
		var loginResult = await accountRepository.PerformExternalLogin();
		return loginResult;
	}

	public async Task<IEnumerable<UserLoginInfo>> GetExternalLogins()
	{
		var userLogins = await accountRepository.GetExternalLogins();
		return userLogins;
	}

	public async Task AddExternalLogin()
	{
		await accountRepository.AddExternalLogin();
	}

	public async Task RemoveExternalLogin(string provider)
	{
		await accountRepository.RemoveExternalLogin(provider);
	}
}
