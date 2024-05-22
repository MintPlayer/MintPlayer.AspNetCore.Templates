using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
// using MintPlayer.AspNetCore.Template.Data.Abstractions.Access.Repositories;
// using MintPlayer.AspNetCore.Template.Data.Abstractions.Access.Services;
// using MintPlayer.AspNetCore.Template.Data.Access.Repositories;
// using MintPlayer.AspNetCore.Template.Data.Access.Services;
// using MintPlayer.AspNetCore.Template.Data.Access.Mappers;

namespace MintPlayer.AspNetCore.Template.Data.Extensions;

public static class ApplicationExtensions
{
	public static IServiceCollection AddMeteo(this IServiceCollection services, Action<Options.MeteoOptions> options)
	{
		var op = new Options.MeteoOptions(); options(op);

		services
			.AddDbContext<Persistance.MeteoContext>(options => options.UseSqlServer(op.ConnectionString));

		services
			.AddIdentity<Persistance.Entities.User, Persistance.Entities.Role>()
			.AddEntityFrameworkStores<Persistance.MeteoContext>()
			.AddDefaultTokenProviders();

		return services;
	}
}
