using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Mappers;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Extensions;

public static class ApplicationExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services, Action<Options.ApplicationOptions> options)
	{
		var op = new Options.ApplicationOptions(); options(op);

		services
			.AddDbContext<Persistance.MeteoContext>(options => options.UseSqlServer(op.ConnectionString))
			.AddScoped<IAccountRepository, AccountRepository>()
			.AddScoped<IAccountService, AccountService>()
			.AddScoped<IUserMapper, UserMapper>()
			.AddScoped<IRoleRepository, RoleRepository>()
			.AddScoped<IRoleService, RoleService>()
			.AddScoped<IRoleMapper, RoleMapper>()
			.AddScoped<IWeatherForecastService, WeatherForecastService>()
			.AddSingleton<IDatabaseService, DatabaseService>();

		services
			.AddIdentity<Persistance.Entities.User, Persistance.Entities.Role>()
			.AddEntityFrameworkStores<Persistance.MeteoContext>()
			.AddDefaultTokenProviders();

		return services;
	}
}
