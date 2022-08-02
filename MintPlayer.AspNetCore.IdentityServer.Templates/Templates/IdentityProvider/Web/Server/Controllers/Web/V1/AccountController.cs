using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using MintPlayer.AspNetCore.MustChangePassword.Exceptions;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.ViewModels.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Controllers.Web.V1;

[Controller]
[Route("Web/V1/[controller]")]
public class AccountController : Controller
{
	#region Constructor
	private readonly IAccountService accountService;
	private readonly IAngularService angularService;
	private readonly UrlEncoder urlEncoder;
	private readonly LinkGenerator linkGenerator;
	private readonly IWebHostEnvironment webHostEnvironment;
	private readonly IServiceProvider serviceProvider;
	public AccountController(
		IAccountService accountService,
		IAngularService angularService,
		UrlEncoder urlEncoder,
		LinkGenerator linkGenerator,
		IWebHostEnvironment webHostEnvironment,
		IServiceProvider serviceProvider)
	{
		this.accountService = accountService;
		this.angularService = angularService;
		this.urlEncoder = urlEncoder;
		this.linkGenerator = linkGenerator;
		this.webHostEnvironment = webHostEnvironment;
		this.serviceProvider = serviceProvider;
	}
	#endregion

	[ValidateAntiForgeryToken]
	[HttpPost("Register", Name = "web-v1-account-register")]
	public async Task<ActionResult<RegisterResult?>> Register([FromBody] RegisterVM registerVM)
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
	public async Task<ActionResult<LoginResult>> Login([FromBody] LoginVM loginVM)
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
		catch (MustChangePasswordException mustChangePasswordEx)
		{
			return Ok(new LoginResult
			{
				Status = Dtos.Enums.ELoginStatus.MustChangePassword,
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
			return Ok(user);
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
	[HttpGet("Password")]
	public async Task<ActionResult<bool>> HasPassword()
	{
		try
		{
			var hasPassword = await accountService.HasPassword();
			return Ok(hasPassword);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}

	[ValidateAntiForgeryToken]
	[HttpPost("Password")]
	public async Task<ActionResult> ForceChangePassword([FromBody] ChangePasswordVM changePasswordVM)
	{
		try
		{
			await accountService.PerformMustChangePassword(changePasswordVM.NewPassword, changePasswordVM.NewPasswordConfirmation);
			return Ok();
		}
		catch (Exception)
		{
			return StatusCode(404);
		}
	}

	[Authorize]
	[ValidateAntiForgeryToken]
	[HttpPut("Password")]
	public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordVM changePasswordVM)
	{
		try
		{
			await accountService.ChangePassword(changePasswordVM.CurrentPassword, changePasswordVM.NewPassword, changePasswordVM.NewPasswordConfirmation);
			return Ok();
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
			const string appName = "MintPlayer.AspNetCore.IdentityServer";
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
			await accountService.SetEnableTwoFactor(twoFactorEnableVM.Enable, twoFactorEnableVM.Code);
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
	[HttpGet("TwoFactor/Recovery/RemainingCodes", Name = "web-v1-account-twofactor-recovery-remainingcodes")]
	public async Task<ActionResult<int>> GetRemainingRecoveryCodes()
	{
		try
		{
			var count = await accountService.GetRemainingRecoveryCodes();
			return Ok(count);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}

	[Authorize]
	[ValidateAntiForgeryToken]
	[HttpPut("TwoFactor/Recovery/RemainingCodes", Name = "web-v1-account-twofactor-recovery-remainingcodes-generate")]
	public async Task<ActionResult<IEnumerable<string>>> GenerateNewRecoveryCodes([FromBody] TwoFactorGenerateCodesVM twoFactorGenerateCodesVM)
	{
		try
		{
			var recoveryCodes = await accountService.GenerateNewTwoFactorRecoveryCodes(twoFactorGenerateCodesVM.VerificationCode);
			return Ok(recoveryCodes);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}

	[Authorize]
	[ValidateAntiForgeryToken]
	[HttpPost("TwoFactor/Bypass", Name = "web-v1-account-twofactor-bypass")]
	public async Task<ActionResult> SetBypassTwoFactorForExternallogins([FromBody] TwoFactorBypassVM twoFactorBypassVM)
	{
		try
		{
			await accountService.SetBypassTwoFactor(twoFactorBypassVM.Bypass, twoFactorBypassVM.VerificationCode);
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

	[ValidateAntiForgeryToken]
	[HttpPost("TwoFactor/Recovery", Name = "web-v1-account-twofactor-recovery")]
	public async Task<ActionResult<User>> TwoFactorRecoveryCodeSignin([FromBody] TwoFactorRecoveryVM twoFactorRecoveryVM)
	{
		try
		{
			var user = await accountService.TwoFactorRecovery(twoFactorRecoveryVM.RecoveryCode);
			return Ok(user);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}

	[HttpGet("ExternalLogin/Providers", Name = "web-v1-account-externallogin-providers")]
	public async Task<ActionResult<IEnumerable<AuthenticationScheme>>> GetExternalLoginProviders()
	{
		try
		{
			var providers = await accountService.GetExternalLoginProviders();
			return Ok(providers);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}

	[HttpGet("ExternalLogin/connect/{provider}", Name = "web-v1-account-externallogin-connect-challenge")]
#if RELEASE
        [Host("external.example.com")]
#endif
	public async Task<ActionResult> ExternalLogin([FromRoute] string provider)
	{
		var redirectUrl = Url.RouteUrl("web-v1-account-externallogin-connect-callback", new { provider });
		var properties = await accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return Challenge(properties, provider);
	}

	[HttpGet("ExternalLogin/connect/{provider}/Callback", Name = "web-v1-account-externallogin-connect-callback")]
#if RELEASE
        [Host("external.example.com")]
#endif
	public async Task<ActionResult> ExternalLoginCallback([FromRoute] string provider)
	{
		try
		{
			var loginResult = await accountService.PerformExternalLogin();
			switch (loginResult.Status)
			{
				case Dtos.Enums.ELoginStatus.Success:
					var successModel = new ExternalLoginResult
					{
						TargetOrigin = $"{Request.Scheme}://{Request.Host.Value.Replace("external.", string.Empty)}",
						User = loginResult.User,
						Status = loginResult.Status,
						Provider = loginResult.Provider,
					};
					return View(successModel);
				case Dtos.Enums.ELoginStatus.RequiresTwoFactor:
					// For external logins, show the two-factor input form in the popup.
					return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider = loginResult.Provider });
				default:
					var failedModel = new ExternalLoginResult
					{
						TargetOrigin = $"{Request.Scheme}://{Request.Host.Value.Replace("external.", string.Empty)}",
						Status = loginResult.Status,
						Provider = loginResult.Provider,
						Error = loginResult.Error,
						ErrorDescription = loginResult.ErrorDescription
					};
					return View(failedModel);
			}
		}
		catch (OtherAccountException otherAccountEx)
		{
			var model = new ExternalLoginResult
			{
				TargetOrigin = $"{Request.Scheme}://{Request.Host.Value.Replace("external.", string.Empty)}",
				Status = Dtos.Enums.ELoginStatus.Failed,
				Provider = provider,
				Error = "Could not login",
				ErrorDescription = otherAccountEx.Message
			};
			return View(model);
		}
		catch (Exception ex)
		{
			var model = new ExternalLoginResult
			{
				TargetOrigin = $"{Request.Scheme}://{Request.Host.Value.Replace("external.", string.Empty)}",
				Status = Dtos.Enums.ELoginStatus.Failed,
				Provider = provider,
				Error = "Could not login",
				ErrorDescription = "There was an error with your social login"
			};
			return View(model);
		}
	}

#if RELEASE
	[Host("external.example.com")]
#endif
	[HttpGet("ExternalLogin/TwoFactor/{provider}", Name = "web-v1-account-externallogin-twofactor")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult> ExternalLoginTwoFactor([FromRoute] string provider)
	{
		var angularStylesheet = await angularService.GetStylesheetUrl(Url);
		var model = new ExternalLoginTwoFactorVM
		{
			SubmitUrl = Url.Action(nameof(ExternalLoginTwoFactorCallback), new { provider })!,
			StylesheetUrl = angularStylesheet
		};
		return View(model);
	}

#if RELEASE
	[Host("external.example.com")]
#endif
	[ValidateAntiForgeryToken]
	[HttpPost("ExternalLogin/TwoFactor/{provider}", Name = "web-v1-account-externallogin-twofactor-callback")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult> ExternalLoginTwoFactorCallback([FromRoute] string provider, [FromForm] ExternalLoginTwoFactorVM externalLoginTwoFactorVM)
	{
		try
		{
			var user = await accountService.TwoFactorLogin(externalLoginTwoFactorVM.Code, externalLoginTwoFactorVM.Remember);
			if (user == null) throw new Exception();

			var successModel = new ExternalLoginResult
			{
				TargetOrigin = $"{Request.Scheme}://{Request.Host.Value.Replace("external.", string.Empty)}",
				Status = Dtos.Enums.ELoginStatus.Success,
				Provider = provider,
				User = user,
			};
			return View(nameof(ExternalLoginCallback), successModel);
		}
		catch (Exception)
		{
			return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider });
		}
	}

	[Authorize]
	[HttpGet("ExternalLogin", Name = "web-v1-account-externallogin-get")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult<IEnumerable<string>>> GetRegisteredExternalLoginProviders()
	{
		try
		{
			var providers = await accountService.GetExternalLogins();
			var providerNames = providers.Select(p => p.LoginProvider).ToArray();
			return Ok(providerNames);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}

	[Authorize]
	[HttpGet("ExternalLogin/add/{provider}", Name = "web-v1-account-externallogin-add-challenge")]
	[ApiExplorerSettings(IgnoreApi = true)]
#if RELEASE
	[Host("external.example.com")]
#endif
	public async Task<ActionResult> AddExternalLogin([FromRoute] string provider)
	{
		var redirectUrl = Url.RouteUrl("web-v1-account-externallogin-add-callback", new { provider });
		var properties = await accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return Challenge(properties, provider);
	}

	[Authorize]
	//[ValidateAntiForgeryToken]
	[HttpGet("ExternalLogin/add/{provider}/Callback", Name = "web-v1-account-externallogin-add-callback")]
	[ApiExplorerSettings(IgnoreApi = true)]
#if RELEASE
	[Host("external.example.com")]
#endif
	public async Task<ActionResult> AddExternalLoginCallback([FromRoute] string provider)
	{
		try
		{
			await accountService.AddExternalLogin();
			var model = new ExternalLoginResult
			{
				TargetOrigin = $"{Request.Scheme}://{Request.Host.Value.Replace("external.", string.Empty)}",
				Status = Dtos.Enums.ELoginStatus.Success,
				Provider = provider,
				User = null,
			};
			return View(model);
		}
		catch (Exception)
		{
			var model = new ExternalLoginResult
			{
				TargetOrigin = $"{Request.Scheme}://{Request.Host.Value.Replace("external.", string.Empty)}",
				Status = Dtos.Enums.ELoginStatus.Failed,
				Provider = provider,
				Error = "Could not login",
				ErrorDescription = "There was an error with your social login",
			};
			return View(model);
		}
	}

	[Authorize]
	[ValidateAntiForgeryToken]
	[HttpDelete("ExternalLogin/{provider}", Name = "web-v1-account-externallogin-delete")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult> DeleteLogin(string provider)
	{
		await accountService.RemoveExternalLogin(provider);
		return Ok();
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
