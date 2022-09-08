using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Controllers.Web.V1;

[Route("Web/V1/[controller]")]
public class SetupController : Controller
{
	#region Constructor
	private readonly ISetupService setupService;
	public SetupController(ISetupService setupService)
	{
		this.setupService = setupService;
	}
	#endregion

	[HttpGet]
	[Authorize(Roles = "Administrator")]
	public async Task<ActionResult<bool>> IsDeveloperPortalRegistered()
	{
		try
		{
			var isRegistered = await setupService.IsDeveloperPortalRegistered();
			return Ok(isRegistered);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}

	[HttpPost]
	[Authorize(Roles = "Administrator")]
#if (UseXsrfProtection)
	[ValidateAntiForgeryToken]
#endif
	public async Task<ActionResult<CreateDeveloperPortalResponse>> CreateDeveloperPortal([FromBody] CreateDeveloperPortalRequest request)
	{
		try
		{
			var response = await setupService.CreateDeveloperClient(request);
			return Ok(response);
		}
		catch (Exception)
		{
			return NotFound();
		}
	}
}
