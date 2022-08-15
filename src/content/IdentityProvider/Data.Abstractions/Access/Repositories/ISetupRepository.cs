using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;

public interface ISetupRepository
{
	Task<bool> IsDeveloperPortalRegistered();
	Task<CreateDeveloperPortalResponse> CreateDeveloperClient(CreateDeveloperPortalRequest request);
}
