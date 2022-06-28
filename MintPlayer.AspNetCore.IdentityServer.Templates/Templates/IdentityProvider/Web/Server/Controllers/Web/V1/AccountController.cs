using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;
using MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Viewmodels.Account;
using Microsoft.AspNetCore.Routing;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Controllers.Web.V1
{
    [Route("Web/V1/[controller]")]
    public class AccountController : Controller
    {
        #region Constructor
        private readonly IAccountService accountService;
        private readonly UrlEncoder urlEncoder;
        private readonly LinkGenerator linkGenerator;
        public AccountController(
            IAccountService accountService,
            UrlEncoder urlEncoder,
            LinkGenerator linkGenerator)
        {
            this.accountService = accountService;
            this.urlEncoder = urlEncoder;
            this.linkGenerator = linkGenerator;
        }
        #endregion

        [ValidateAntiForgeryToken]
        [HttpPost("Register", Name = "web-v1-account-register")]
        public async Task<ActionResult<Dtos.Dtos.User>> Register([FromBody] RegisterVM registerVM)
        {
            try
            {
                var result = await accountService.Register(registerVM.User, registerVM.Password, registerVM.PasswordConfirmation);
                var confirmationToken = await accountService.GenerateEmailConfirmationToken(registerVM.User.Email);
                var confirmationUrl = linkGenerator.GetUriByName("web-v1-account-verify", new
                {
                    email = registerVM.User.Email,
                    code = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Encode(System.Text.Encoding.UTF8.GetBytes(confirmationToken)),
                }, Request.Scheme, Request.Host);

                await accountService.SendConfirmationEmail(registerVM.User.Email, confirmationUrl);
                return Ok(result);
            }
            catch (PasswordConfirmationException passwordEx)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet("Verify", Name = "web-v1-account-verify")]
        public async Task<ActionResult> Verify([FromQuery] string email, [FromQuery] string code)
        {
            try
            {
                var decodedCode = System.Text.Encoding.UTF8.GetString(Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Decode(code));
                await accountService.VerifyEmailConfirmationToken(email, decodedCode);
                var loginUrl = "/account/login"; // TODO: Generate url for SPA
                return Redirect(loginUrl);
            }
            catch (VerifyEmailException verifyEx)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost("Resend", Name = "web-v1-account-resend")]
        public async Task<ActionResult> ResendConfirmationEmail([FromBody] ResendConfirmationEmailVM resendConfirmationEmailVM)
        {
            try
            {
                var confirmationToken = await accountService.GenerateEmailConfirmationToken(resendConfirmationEmailVM.Email);
                var confirmationUrl = linkGenerator.GetUriByName("web-v1-account-verify", new
                {
                    email = resendConfirmationEmailVM.Email,
                    code = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Encode(System.Text.Encoding.UTF8.GetBytes(confirmationToken)),
                }, Request.Scheme, Request.Host);

                await accountService.SendConfirmationEmail(resendConfirmationEmailVM.Email, confirmationUrl);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
                throw;
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost("Login", Name = "web-v1-account-login")]
        public async Task<ActionResult> Login([FromBody] LoginVM loginVM)
        {
            try
            {
                var result = await accountService.Login(loginVM.Email, loginVM.Password, true);
                return Ok(new LoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.Success,
                    User = result
                });
            }
            catch (LoginException loginEx)
            {
                return StatusCode(403, new LoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.Failed,
                    User = null
                });
            }
            catch (InvalidPasswordException passwordEx)
            {
                return StatusCode(403, new LoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.Failed,
                    User = null
                });
            }
            catch (EmailNotConfirmedException emailNotConfirmedEx)
            {
                return StatusCode(403, new LoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.NotActivated,
                    User = null
                });
            }
            catch (RequiresTwoFactorException twoFactorEx)
            {
                return Ok(new LoginResult
                {
                    Status = Dtos.Enums.ELoginStatus.RequiresTwoFactor,
                    User = twoFactorEx.User
                });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [ValidateAntiForgeryToken]
        [HttpPost("TwoFactor/Login", Name = "web-v1-account-twofactor-login")]
        public async Task<ActionResult<User>> TwoFactorLogin([FromBody] TwoFactorLoginVM twoFactorLoginVM)
        {
            try
            {
                var user = await accountService.TwoFactorLogin(twoFactorLoginVM.VerificationCode, twoFactorLoginVM.RememberDevice);
                return user;
            }
            catch (LoginException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("CurrentUser", Name = "web-v1-account-currentuser")]
        public async Task<ActionResult<Dtos.Dtos.User>> CurrentUser()
        {
            try
            {
                var user = await accountService.GetCurrentUser();
                return Ok(user);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("Roles", Name = "web-v1-account-roles")]
        public async Task<ActionResult<IEnumerable<string>>> Roles()
        {
            try
            {
                var roles = await accountService.GetRoles();
                return Ok(roles);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("TwoFactor/RegistrationInfo", Name = "web-v1-account-twofactor-registrationinfo")]
        public async Task<ActionResult<TwoFactorRegistrationInfo>> GetTwoFactorRegistrationInfo()
        {
            try
            {
                const string appName = "VehicleStudio";
                var user = await accountService.GetCurrentUser();
                var registrationCode = await accountService.GenerateTwoFactorRegistrationCode();
                var registrationUrl = $"otpauth://totp/{urlEncoder.Encode(appName)}:{urlEncoder.Encode(user.Email)}?secret={registrationCode}&issuer={urlEncoder.Encode(appName)}&digits=8";

                return Ok(new TwoFactorRegistrationInfo
                {
                    RegistrationUrl = registrationUrl,
                });
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("TwoFactor", Name = "web-v1-account-twofactor-enable")]
        public async Task<ActionResult> SetEnableTwoFactor([FromBody] TwoFactorEnableVM twoFactorEnableVM)
        {
            try
            {
                await accountService.SetEnableTwoFactor(twoFactorEnableVM.Enable, twoFactorEnableVM.VerificationCode);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("Logout", Name = "web-v1-account-logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await accountService.Logout();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpPost("Csrf-Refresh", Name = "web-v1-account-csrfrefresh")]
        public async Task<ActionResult> CsrfRefresh()
        {
            // Just an empty method that returns a new cookie with a new CSRF token.
            // Call this method when the user has signed in/out.

            await Task.Delay(5);
            return Ok();
        }
    }
}
