using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Mappers;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Access.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Extensions;

public static class SsoExtensions
{
	public static IServiceCollection AddSso(this IServiceCollection services, Action<Options.SsoOptions> options)
	{
		var op = new Options.SsoOptions(); options(op);

		services
			.AddDbContext<Persistance.SsoContext>(options => options.UseSqlServer(op.ConnectionString))
			.AddScoped<IAccountRepository, AccountRepository>()
			.AddScoped<IAccountService, AccountService>()
			.AddScoped<IUserMapper, UserMapper>()
			.AddScoped<IRoleRepository, RoleRepository>()
			.AddScoped<IRoleService, RoleService>()
			.AddScoped<IRoleMapper, RoleMapper>()
			.AddSingleton<IDatabaseService, DatabaseService>();

		services
			.AddIdentity<Persistance.Entities.User, Persistance.Entities.Role>()
			.AddEntityFrameworkStores<Persistance.SsoContext>()
			.AddDefaultTokenProviders();

		var idsrvBuilder = services.AddIdentityServer()
			.AddOperationalStore<Persistance.SsoContext>(isOptions =>
			{
				isOptions.ConfigureDbContext = (builder) => builder.UseSqlServer(op.ConnectionString);
			})
			.AddConfigurationStore<Persistance.SsoContext>(isOptions =>
			{
				isOptions.ConfigureDbContext = (builder) => builder.UseSqlServer(op.ConnectionString);
			})
			.AddAspNetIdentity<Persistance.Entities.User>()
			.AddProfileService<Services.SsoProfileService>();

		if (op.Environment.IsDevelopment())
		{
			idsrvBuilder.AddDeveloperSigningCredential();
		}
		else
		{
			//idsrvBuilder.AddSigningCredential()
		}

		return services;
	}
}
