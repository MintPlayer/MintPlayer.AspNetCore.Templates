using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
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
    private readonly LinkGenerator linkGenerator;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IMailService mailService;
    private readonly IOptions<SmtpOptions> smtpOptions;
    public AccountService(
        IAccountRepository accountRepository,
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor,
        IMailService mailService,
        IOptions<Options.SmtpOptions> smtpOptions)
    {
        this.accountRepository = accountRepository;
        this.linkGenerator = linkGenerator;
        this.httpContextAccessor = httpContextAccessor;
        this.mailService = mailService;
        this.smtpOptions = smtpOptions;
    }
    #endregion

    public async Task<User> Register(User user, string password, string passwordConfirmation)
    {
        if (password != passwordConfirmation)
        {
            throw new PasswordConfirmationException();
        }

        var newUser = await accountRepository.Register(user, password);
        return newUser;
    }

    public async Task<string> GenerateEmailConfirmationToken(string email)
    {
        var token = await accountRepository.GenerateEmailConfirmationToken(email);
        return token;
    }

    public async Task SendConfirmationEmail(string email, string confirmationUrl)
    {
        var html = $@"Please confirm your account by clicking <a href=""{confirmationUrl}"">here</a>.";
        using (var client = await mailService.CreateSmtpClient())
        using (var message = new MailMessage(smtpOptions.Value.DefaultFrom, email, "Confirm email address", html))
        {
            message.IsBodyHtml = true;
            client.Send(message);
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

    public async Task<string> GenerateTwoFactorRegistrationCode()
    {
        var code = await accountRepository.GenerateTwoFactorRegistrationCode();
        return code;
    }

    public async Task SetEnableTwoFactor(bool enable, string verificationCode)
    {
        await accountRepository.SetEnableTwoFactor(enable, verificationCode);
    }
}
