using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

public interface IAccountService
{
	Task<RegisterResult> Register(User user, string password, string passwordConfirmation);
#if (UseEmailConfirmation)
	Task<string> GenerateEmailConfirmationToken(string email);
	Task SendConfirmationEmail(string email, string confirmationUrl);
	Task VerifyEmailConfirmationToken(string email, string token);
#endif
	Task<User> Login(string email, string password, bool createCookie);
	Task<User> GetCurrentUser();
	Task<IEnumerable<string>> GetRoles();
	Task Logout();
	Task<bool> HasPassword();
	Task PerformMustChangePassword(string newPassword, string newPasswordConfirmation);
	Task ChangePassword(string? currentPassword, string newPassword, string newPasswordConfirmation);
#if (UseTwoFactorAuthentication)
	Task<string> GenerateTwoFactorRegistrationCode();
	Task<User> TwoFactorLogin(string verificationCode, bool rememberDevice);
	Task SetEnableTwoFactor(bool enable, TwoFactorCode code);
	Task<int> GetRemainingRecoveryCodes();
	Task SetBypassTwoFactor(bool bypass, string verificationCode);
	Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodes(string verificationCode);
	Task<User> TwoFactorRecovery(string recoveryCode);
#endif
#if (UseExternalLogins)
	Task<IEnumerable<AuthenticationScheme>> GetExternalLoginProviders();
	Task<AuthenticationProperties> ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
	Task<ExternalLoginResult> PerformExternalLogin();
	Task<IEnumerable<UserLoginInfo>> GetExternalLogins();
	Task AddExternalLogin();
	Task RemoveExternalLogin(string provider);
#endif
}
