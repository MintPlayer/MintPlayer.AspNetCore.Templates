using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

public interface IAccountService
{
    Task<User> Register(User user, string password, string passwordConfirmation);
    Task<string> GenerateEmailConfirmationToken(string email);
    Task SendConfirmationEmail(string email, string confirmationUrl);
    Task VerifyEmailConfirmationToken(string email, string token);
    Task<User> Login(string email, string password, bool createCookie);
    Task<User> GetCurrentUser();
    Task<IEnumerable<string>> GetRoles();
    Task Logout();
    Task<string> GenerateTwoFactorRegistrationCode();
    Task<User> TwoFactorLogin(string verificationCode, bool rememberDevice);
    Task SetEnableTwoFactor(bool enable, string verificationCode);
}
