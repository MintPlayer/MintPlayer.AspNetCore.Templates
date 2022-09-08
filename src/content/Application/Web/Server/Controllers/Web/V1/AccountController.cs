using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Web.Server.Controllers.Web.V1;

[Controller]
[Route("Web/V1/[controller]")]
public class AccountController : Controller
{
	#region Constructor
	private readonly IAccountService accountService;
	public AccountController(IAccountService accountService)
	{
		this.accountService = accountService;
	}
	#endregion

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
	public async Task<ActionResult> ExternalLogin([FromRoute] string provider)
	{
		var redirectUrl = Url.RouteUrl("web-v1-account-externallogin-connect-callback", new { provider });
		var properties = await accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
		return Challenge(properties, provider);
	}

	[HttpGet("ExternalLogin/connect/{provider}/Callback", Name = "web-v1-account-externallogin-connect-callback")]
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
				//case Dtos.Enums.ELoginStatus.RequiresTwoFactor:
				//	// For external logins, show the two-factor input form in the popup.
				//	return RedirectToAction(nameof(ExternalLoginTwoFactor), new { provider = loginResult.Provider });
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

	[Authorize]
#if (UseXsrfProtection)
	[ValidateAntiForgeryToken]
#endif
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

#if (UseXsrfProtection)
	[HttpPost("Csrf-Refresh", Name = "web-v1-account-csrfrefresh")]
	public async Task<ActionResult> CsrfRefresh()
	{
		// Just an empty method that returns a new cookie with a new CSRF token.
		// Call this method when the user has signed in/out.

		await Task.Delay(5);
		return Ok();
	}

#endif
}
