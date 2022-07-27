using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;
using MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.ViewModels.Account;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Controllers.Api.V1
{
	[ApiController]
	[Route("Api/V1/[controller]")]
	public class AccountController : Controller
	{
		#region Constructor
		private readonly IAccountService accountService;
		public AccountController(IAccountService accountService)
		{
			this.accountService = accountService;
		}
		#endregion

		[HttpPost("Register", Name = "api-v1-account-register")]
		public async Task<ActionResult<RegisterResult?>> Register([FromBody] RegisterVM registerVM)
		{
			try
			{
				var result = await accountService.Register(registerVM.User, registerVM.Password, registerVM.PasswordConfirmation);
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

		[HttpPost("login", Name = "api-v1-account-login")]
		public async Task<ActionResult<User?>> Login([FromBody] LoginVM loginInfo)
		{
			try
			{
				var user = await accountService.Login(loginInfo.Email, loginInfo.Password, false);
				return Ok(user);
			}
			catch (LoginException loginEx)
			{
				return Unauthorized();
			}
			catch (Exception ex)
			{
				return StatusCode(500);
			}
		}

		[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
		[HttpGet("CurrentUser")]
		public async Task<ActionResult<User>> CurrentUser()
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

		[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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

		[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
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
	}
}
