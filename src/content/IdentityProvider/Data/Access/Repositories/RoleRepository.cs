using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Mappers;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;
using MintPlayer.Extensions.OrderBy;
using MintPlayer.Pagination;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Repositories;

internal class RoleRepository : IRoleRepository
{
	#region Constructor
	private readonly RoleManager<Persistance.Entities.Role> roleManager;
	private readonly IRoleMapper roleMapper;
	private readonly Persistance.SsoContext ssoContext;
	public RoleRepository(
		RoleManager<Persistance.Entities.Role> roleManager,
		IRoleMapper roleMapper,
		Persistance.SsoContext ssoContext)
	{
		this.roleManager = roleManager;
		this.roleMapper = roleMapper;
		this.ssoContext = ssoContext;
	}
	#endregion

	public async Task<Role> GetRole(Guid roleId)
	{
		var role = await ssoContext.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
		var dto = await roleMapper.Entity2Dto(role);
		return dto;
	}

	public async Task<PaginationResponse<Role>> PageRoles(PaginationRequest<Role> request)
	{
		var roles = ssoContext.Roles;

		// 1) Sort
		var orderedRoles = request.SortDirection == System.ComponentModel.ListSortDirection.Descending
			? roles.OrderByDescending(request.SortProperty)
			: roles.OrderBy(request.SortProperty);

		// 2) Page
		var pagedRoles = orderedRoles
			.Skip((request.Page - 1) * request.PerPage)
			.Take(request.PerPage);

		// 3) Convert to DTO
		var dtoRoles = await Task.WhenAll(pagedRoles.Select(role => roleMapper.Entity2Dto(role)));

		if (dtoRoles == null)
		{
			throw new InvalidOperationException("Unexpected. dtoRoles should not be null");
		}

		var countRoles = await ssoContext.Roles.CountAsync();
		return new PaginationResponse<Role>(request, countRoles, dtoRoles);
	}

	public async Task<IEnumerable<Role>> GetRoles()
	{
		var roles = await Task.WhenAll(ssoContext.Roles
			.Select(r => roleMapper.Entity2Dto(r)));
		return roles;
	}

	public async Task<Role> CreateRole(string roleName)
	{
		var entityRole = new Persistance.Entities.Role { Name = roleName };
		var result = await roleManager.CreateAsync(entityRole);
		if (result.Succeeded)
		{
			var dto = await roleMapper.Entity2Dto(entityRole);
			return dto;
		}
		else
		{
			throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
		}
	}

	public async Task<Role> UpdateRole(Guid id, Role role)
	{
		var entityRole = await ssoContext.Roles.FindAsync(id);
		if (entityRole == null)
		{
			throw new Exception("Role not found");
		}

		entityRole.Name = role.Name;
		var result = await roleManager.UpdateAsync(entityRole);
		if (result.Succeeded)
		{
			var dto = await roleMapper.Entity2Dto(entityRole);
			return dto;
		}
		else
		{
			throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
		}
	}

	public async Task DeleteRole(Guid id)
	{
		var entityRole = await ssoContext.Roles.FindAsync(id);
		if (entityRole == null)
		{
			throw new Exception("Role not found");
		}

		await roleManager.DeleteAsync(entityRole);
	}
}
