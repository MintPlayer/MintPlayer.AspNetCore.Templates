using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
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
		var externalProviders = await accountService.GetExternalLoginProviders();
		var stylesheetUrl = await angularService.GetStylesheetUrl(Url);

		return View(new Provider.Server.ViewModels.Provider.Auth.LoginVM
		{
			ReturnUrl = returnUrl,
			User = new User
			{
				UserName = context?.LoginHint ?? string.Empty,
			},
			ExternalProviders = externalProviders,
			StylesheetUrl = stylesheetUrl,
		});
	}

	[HttpPost("Login")]
	[ValidateAntiForgeryToken]
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
		catch (RequiresTwoFactorException twoFaEx)
		{
			return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider = "local", returnUrl = model.ReturnUrl });
		}
		catch (Exception ex)
		{
			return View(model);
		}
	}

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
				case Dtos.Enums.ELoginStatus.RequiresTwoFactor:
					// For external logins, show the two-factor input form in the popup.
					return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider = loginResult.Provider, returnUrl });
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

	[HttpGet("ExternalLogin/TwoFactor/{provider}", Name = "auth-externallogin-twofactor")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult> ExternalLoginTwoFactor([FromRoute] string provider, [FromQuery] string returnUrl)
	{
		var stylesheetUrl = await angularService.GetStylesheetUrl(Url);
		var model = new Web.Server.ViewModels.Account.ExternalLoginTwoFactorVM
		{
			SubmitUrl = returnUrl,
			StylesheetUrl = stylesheetUrl,
		};
		return View(model);
	}

	[ValidateAntiForgeryToken]
	[HttpPost("ExternalLogin/TwoFactor/{provider}", Name = "auth-externallogin-twofactor-callback")]
	[ApiExplorerSettings(IgnoreApi = true)]
	public async Task<ActionResult> ExternalLoginTwoFactorCallback([FromRoute] string provider, [FromForm] Web.Server.ViewModels.Account.ExternalLoginTwoFactorVM externalLoginTwoFactorVM)
	{
		try
		{
			var user = await accountService.TwoFactorLogin(externalLoginTwoFactorVM.Code, externalLoginTwoFactorVM.Remember);
			if (user == null) throw new Exception();

			return Redirect(externalLoginTwoFactorVM.SubmitUrl);
		}
		catch (Exception)
		{
			return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider });
		}
	}
}
