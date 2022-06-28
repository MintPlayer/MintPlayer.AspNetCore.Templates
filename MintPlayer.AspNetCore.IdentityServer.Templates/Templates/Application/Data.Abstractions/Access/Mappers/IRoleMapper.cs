namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Mappers;

internal interface IRoleMapper
{
    Task<Persistance.Entities.Role> Dto2Entity(Dtos.Dtos.Role role);
    Task<Dtos.Dtos.Role> Entity2Dto(Persistance.Entities.Role role);
}