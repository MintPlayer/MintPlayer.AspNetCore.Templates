using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;

public interface IAccountRepository
{
	public Task<User> Register(User user, string password);
#if (UseEmailConfirmation)
	Task<string> GenerateEmailConfirmationToken(string email);
	Task VerifyEmailConfirmationToken(string email, string token);
#endif
	Task<User> Login(string email, string password, bool createCookie);
#if (UseTwoFactorAuthentication)
	Task<User> TwoFactorLogin(string verificationCode, bool rememberDevice);
#endif
	Task<User> GetCurrentUser();
	Task<IEnumerable<string>> GetRoles();
	Task Logout();
	Task<bool> HasPassword();
	Task ChangePassword(string? currentPassword, string newPassword, string newPasswordConfirmation);
#if (UseTwoFactorAuthentication)
	Task<string> GenerateTwoFactorRegistrationCode();
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
