using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MintPlayer.AspNetCore.Template.Data.Persistance.Entities;

namespace MintPlayer.AspNetCore.Template.Data.Persistance;

internal class MeteoContext : IdentityDbContext<User, Role, Guid>
{
	#region Constructor
	private readonly IConfiguration? configuration;
	public MeteoContext()
	{
		configuration = null;
	}
	public MeteoContext(IConfiguration configuration)
	{
		this.configuration = configuration;
	}
	#endregion

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		if (configuration == null)
		{
			// Only used when generating migrations
			var migrationsConnectionString = @"Server=(localdb)\mssqllocaldb;Database=MintPlayer.AspNetCore.Template;Trusted_Connection=True;ConnectRetryCount=0";
			optionsBuilder.UseSqlServer(migrationsConnectionString, options =>
			{
				options.MigrationsAssembly(typeof(MeteoContext).Assembly.FullName);
			});
		}
		else
		{
			optionsBuilder.UseSqlServer(configuration.GetConnectionString("MintPlayer.AspNetCore.Template"));
		}
	}
}
