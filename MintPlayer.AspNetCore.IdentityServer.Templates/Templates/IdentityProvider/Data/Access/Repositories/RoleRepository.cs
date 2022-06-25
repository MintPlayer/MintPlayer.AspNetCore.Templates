using Microsoft.AspNetCore.Identity;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Repositories;

internal class RoleRepository : IRoleRepository
{
    #region Constructor
    private readonly RoleManager<Role> roleManager;
    public RoleRepository(RoleManager<Persistance.Entities.Role> roleManager)
    {
        this.roleManager = roleManager;
    }
    #endregion
}
