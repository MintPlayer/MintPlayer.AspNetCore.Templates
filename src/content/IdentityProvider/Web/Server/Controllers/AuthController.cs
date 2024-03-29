using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Server.Controllers;

[Route("Account")]
public class AuthController : Controller
{
	#region Constructor
	private readonly IIdentityServerInteractionService interaction;
	private readonly IAccountService accountService;
	private readonly IAngularService angularService;
	private readonly ISsoService ssoService;
	public AuthController(
		IIdentityServerInteractionService interaction,
		IAccountService accountService,
		IAngularService angularService,
		ISsoService ssoService)
	{
		this.interaction = interaction;
		this.accountService = accountService;
		this.angularService = angularService;
		this.ssoService = ssoService;
	}
	#endregion

	[HttpGet("Login")]
	public async Task<IActionResult> AppLogin([FromQuery] string returnUrl)
	{
		var context = await interaction.GetAuthorizationContextAsync(returnUrl);
#if (UseExternalLogins)
		var externalProviders = await accountService.GetExternalLoginProviders();
#endif
		var stylesheetUrl = await angularService.GetStylesheetUrl(Url);

		return View(new Provider.Server.ViewModels.Provider.Auth.LoginVM
		{
			ReturnUrl = returnUrl,
			User = new User
			{
				UserName = context?.LoginHint ?? string.Empty,
			},
#if (UseExternalLogins)
			ExternalProviders = externalProviders,
#endif
			StylesheetUrl = stylesheetUrl,
		});
	}

	[HttpPost("Login")]
#if (UseXsrfProtection)
	[ValidateAntiForgeryToken]
#endif
	public async Task<IActionResult> LoginPost([FromForm] Provider.Server.ViewModels.Provider.Auth.LoginVM model)
	{
		try
		{
			var request = await ssoService.Login(model.User.Email, model.Password, model.ReturnUrl);
			if (request != null)
			{
				return Redirect(model.ReturnUrl);
			}
			else if (Url.IsLocalUrl(model.ReturnUrl))
			{
				return Redirect(model.ReturnUrl);
			}
			else if (string.IsNullOrEmpty(model.ReturnUrl))
			{
				return Redirect("~/");
			}
			else
			{
				throw new Exception("Invalid return url");
			}
		}
#if (UseTwoFactorAuthentication)
		catch (RequiresTwoFactorException twoFaEx)
		{
			return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider = "local", returnUrl = model.ReturnUrl });
		}
#endif
		catch (Exception ex)
		{
			return View(model);
		}
	}

#if (UseExternalLogins)
	[HttpGet("ExternalLogin/{provider}", Name = "auth-externallogin-challenge")]
	public async Task<ActionResult> ExternalLogin([FromRoute] string provider, [FromQuery] string returnUrl)
	{
		var redirectUrl = Url.RouteUrl("auth-externallogin-callback", new { provider, returnUrl });
		var properties = await accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return Challenge(properties, provider);
	}

	[HttpGet("ExternalLogin/{provider}/Callback", Name = "auth-externallogin-callback")]
	public async Task<ActionResult> ExternalLoginCallback([FromRoute] string provider, [FromQuery] string returnUrl)
	{
		try
		{
			var loginResult = await accountService.PerformExternalLogin();
			switch (loginResult.Status)
			{
				case Dtos.Enums.ELoginStatus.Success:
					return Redirect(returnUrl);
#if (UseTwoFactorAuthentication)
				case Dtos.Enums.ELoginStatus.RequiresTwoFactor:
					// For external logins, show the two-factor input form in the popup.
					return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider = loginResult.Provider, returnUrl });
#endif
				default:
					return StatusCode(500);
			}
		}
		catch (OtherAccountException otherAccountEx)
		{
			return StatusCode(500);
		}
		catch (Exception ex)
		{
			return StatusCode(500);
		}
	}

#if (UseTwoFactorAuthentication)
	[HttpGet("ExternalLogin/TwoFactor/{provider}", Name = "auth-externallogin-twofactor")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult> ExternalLoginTwoFactor([FromRoute] string provider, [FromQuery] string returnUrl)
	{
		var stylesheetUrl = await angularService.GetStylesheetUrl(Url);
		var model = new Web.Server.ViewModels.Account.ExternalLoginTwoFactorVM
		{
			// It might be better to store the returnUrl in an httponly cookie
			SubmitUrl = Url.Action(nameof(ExternalLoginTwoFactorCallback), new { provider, returnUrl })!,
			StylesheetUrl = stylesheetUrl,
		};
		return View(model);
	}

#if (UseXsrfProtection)
	[ValidateAntiForgeryToken]
#endif
	[HttpPost("ExternalLogin/TwoFactor/{provider}", Name = "auth-externallogin-twofactor-callback")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult> ExternalLoginTwoFactorCallback([FromRoute] string provider, [FromForm] Web.Server.ViewModels.Account.ExternalLoginTwoFactorVM externalLoginTwoFactorVM, [FromQuery] string returnUrl)
	{
		try
		{
			var user = await accountService.TwoFactorLogin(externalLoginTwoFactorVM.Code, externalLoginTwoFactorVM.Remember);
			if (user == null) throw new Exception();

			return Redirect(returnUrl);
		}
		catch (Exception)
		{
			return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider });
		}
	}
#endif
#endif
}
