using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

internal class SetupService : ISetupService
{
	#region Constructor
	private readonly ISetupRepository setupRepository;
	public SetupService(ISetupRepository setupRepository)
	{
		this.setupRepository = setupRepository;
	}
	#endregion

	public async Task<bool> IsDeveloperPortalRegistered()
	{
		var isRegistered = await setupRepository.IsDeveloperPortalRegistered();
		return isRegistered;
	}

	public async Task<CreateDeveloperPortalResponse> CreateDeveloperClient(CreateDeveloperPortalRequest request)
	{
		var result = await setupRepository.CreateDeveloperClient(request);
		return result;
	}
}
