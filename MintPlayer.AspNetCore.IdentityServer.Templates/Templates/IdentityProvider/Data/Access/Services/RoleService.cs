using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

internal class RoleService : IRoleService
{
    #region Constructor
    private readonly IRoleRepository roleRepository;
    public RoleService(IRoleRepository roleRepository)
    {
        this.roleRepository = roleRepository;
    }
    #endregion
}
