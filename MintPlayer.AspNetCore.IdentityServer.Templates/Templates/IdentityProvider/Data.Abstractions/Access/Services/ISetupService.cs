using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

public interface ISetupService
{
	Task<bool> IsDeveloperPortalRegistered();
	Task<CreateDeveloperPortalResponse> CreateDeveloperClient(CreateDeveloperPortalRequest request);
}
