using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web.Server.Controllers.Api.V1
{
    [ApiController]
    [Route("Api/V1/[controller]")]
    public class RoleController : Controller
    {
        #region Constructor
        private readonly IRoleService roleService;
        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }
        #endregion

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> Get()
        {
            var roles = await roleService.GetRoles();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> Get(Guid id)
        {
            var role = await roleService.GetRole(id);
            return Ok(role);
        }

        [HttpPost]
        [Authorize(Roles = "administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Role>> Post([FromBody] Role role)
        {
            var result = await roleService.CreateRole(role.Name);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Role>> Put([FromRoute] Guid id, [FromBody] Role role)
        {
            var result = await roleService.UpdateRole(id, role);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "administrator")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await roleService.DeleteRole(id);
            return Ok();
        }
    }
}
