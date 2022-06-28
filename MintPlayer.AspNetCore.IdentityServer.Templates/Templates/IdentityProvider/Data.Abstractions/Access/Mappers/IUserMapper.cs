namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Mappers;

internal interface IUserMapper
{
    Task<Persistance.Entities.User> Dto2Entity(Dtos.Dtos.User user);
    Task<Dtos.Dtos.User> Entity2Dto(Persistance.Entities.User user);
}