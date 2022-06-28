using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Mappers;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Repositories;

internal class AccountRepository : IAccountRepository
{
    #region Constructor
    private readonly UserManager<Persistance.Entities.User> userManager;
    private readonly SignInManager<Persistance.Entities.User> signinManager;
    private readonly IUserMapper userMapper;
    private readonly IHttpContextAccessor httpContextAccessor;
    public AccountRepository(
        UserManager<Persistance.Entities.User> userManager,
        SignInManager<Persistance.Entities.User> signinManager,
        IUserMapper userMapper,
        IHttpContextAccessor httpContextAccessor)
    {
        this.userManager = userManager;
        this.signinManager = signinManager;
        this.userMapper = userMapper;
        this.httpContextAccessor = httpContextAccessor;
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

    public async Task<Dtos.Dtos.User> Login(string email, string password, bool createCookie)
    {
        try
        {
            var user = await signinManager.UserManager.FindByEmailAsync(email);
            if (user == null) throw new LoginException { Email = email };

            if (createCookie)
            {
                var isPasswordCorrect = await userManager.CheckPasswordAsync(user, password);
                if (!isPasswordCorrect) throw new InvalidPasswordException();

                var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed) throw new EmailNotConfirmedException();

                var signinResult = await signinManager.PasswordSignInAsync(user, password, true, true);
                if (signinResult.Succeeded)
                {
                    var dto = await userMapper.Entity2Dto(user);
                    return dto;
                }
                else if (signinResult.RequiresTwoFactor)
                {
                    throw new RequiresTwoFactorException
                    {
                        User = await userMapper.Entity2Dto(user)
                    };
                }
                else
                {
                    throw new LoginException();
                }
            }
            else
            {
                var signinResult = await signinManager.CheckPasswordSignInAsync(user, password, true);
                if (!signinResult.Succeeded) throw new LoginException { UserId = user.Id, Email = email, Username = user.UserName };

                var isEmailConfirmed = await userManager.IsEmailConfirmedAsync(user);
                if (!isEmailConfirmed) throw new EmailNotConfirmedException();

                if (signinResult.Succeeded)
                {
                    var dto = await userMapper.Entity2Dto(user);
                    return dto;
                }
                else if (signinResult.RequiresTwoFactor)
                {
                    throw new RequiresTwoFactorException
                    {
                        User = await userMapper.Entity2Dto(user)
                    };
                }
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

    public async Task SetEnableTwoFactor(bool enable, string verificationCode)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext.User);
        var isCodeCorrect = await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

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
}
