namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Mappers;

internal class RoleMapper : IRoleMapper
{
    public Task<Persistance.Entities.Role> Dto2Entity(Dtos.Dtos.Role role)
    {
        if (role == null)
        {
            return null;
        }
        else
        {
            return Task.FromResult(new Persistance.Entities.Role
            {
                Id = role.Id,
                Name = role.Name,
            });
        }
    }

    public Task<Dtos.Dtos.Role> Entity2Dto(Persistance.Entities.Role role)
    {
        if (role == null)
        {
            return null;
        }
        else
        {
            return Task.FromResult(new Dtos.Dtos.Role
            {
                Id = role.Id,
                Name = role.Name,
            });
        }
    }
}