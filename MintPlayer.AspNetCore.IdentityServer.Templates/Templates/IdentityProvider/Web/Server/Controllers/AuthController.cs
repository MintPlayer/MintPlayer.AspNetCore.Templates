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
	private readonly ISsoService ssoService;
	public AuthController(
		IIdentityServerInteractionService interaction,
		ISsoService ssoService)
	{
		this.interaction = interaction;
		this.ssoService = ssoService;
	}
	#endregion

	[HttpGet("Login")]
	public async Task<IActionResult> AppLogin([FromQuery] string returnUrl)
	{
		var context = await interaction.GetAuthorizationContextAsync(returnUrl);

		return View(new Provider.Server.ViewModels.Provider.Auth.LoginVM
		{
			ReturnUrl = returnUrl,
			User = new User
			{
				UserName = context?.LoginHint ?? string.Empty,
			}
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
		catch (Exception ex)
		{
			return View(model);
		}
	}
}
