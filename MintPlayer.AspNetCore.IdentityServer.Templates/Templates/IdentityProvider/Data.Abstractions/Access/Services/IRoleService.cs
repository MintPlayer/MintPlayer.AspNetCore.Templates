using MintPlayer.Pagination;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

public interface IRoleService
{
	Task<IEnumerable<Dtos.Dtos.Role>> GetRoles();
	Task<PaginationResponse<Dtos.Dtos.Role>> PageRoles(PaginationRequest<Dtos.Dtos.Role> request);
	Task<Dtos.Dtos.Role> GetRole(Guid roleId);
	Task<Dtos.Dtos.Role> CreateRole(string roleName);
	Task<Dtos.Dtos.Role> UpdateRole(Guid id, Dtos.Dtos.Role role);
	Task DeleteRole(Guid id);
}
