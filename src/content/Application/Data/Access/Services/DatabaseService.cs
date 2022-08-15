using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Persistance;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Services;

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
			scope.ServiceProvider.GetRequiredService<MeteoContext>().Database.Migrate();
		}
	}
}
