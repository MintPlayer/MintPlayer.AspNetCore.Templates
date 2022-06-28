using MintPlayer.Pagination;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

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

    public async Task<PaginationResponse<Role>> PageRoles(PaginationRequest<Role> request)
    {
        var result = await roleRepository.PageRoles(request);
        return result;
    }

    public async Task<IEnumerable<Role>> GetRoles()
    {
        var roles = await roleRepository.GetRoles();
        return roles;
    }

    public async Task<Role> GetRole(Guid roleId)
    {
        var role = await roleRepository.GetRole(roleId);
        return role;
    }

    public async Task<Role> CreateRole(string roleName)
    {
        var role = await roleRepository.CreateRole(roleName);
        return role;
    }

    public async Task<Role> UpdateRole(Guid id, Role role)
    {
        var updatedRole = await roleRepository.UpdateRole(id, role);
        return updatedRole;
    }

    public async Task DeleteRole(Guid id)
    {
        await roleRepository.DeleteRole(id);
    }
}
