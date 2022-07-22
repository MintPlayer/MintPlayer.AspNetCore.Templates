using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

internal class DatabaseService : IDatabaseService
{
	#region Constructor
	private readonly IServiceProvider serviceProvider;
	public DatabaseService(IServiceProvider serviceProvider)
	{
		this.serviceProvider = serviceProvider;
	}
	#endregion

	public void Migrate()
	{
		using (var scope = serviceProvider.CreateScope())
		{
			scope.ServiceProvider.GetRequiredService<SsoContext>().Database.Migrate();
		}
	}
}
