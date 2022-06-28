using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Exceptions.Account;
using MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Viewmodels.Account;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Controllers.Api.V1
{
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
		public async Task<ActionResult> Register([FromBody] RegisterVM registerVM)
		{
			try
			{
				await accountService.Register(registerVM.User, registerVM.Password, registerVM.PasswordConfirmation);
				return Ok();
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
		public async Task<ActionResult> Login([FromBody] LoginVM loginInfo)
		{
			try
			{
				var login_result = await accountService.Login(loginInfo.Email, loginInfo.Password, false);
				return Ok(login_result);
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
	}
}
