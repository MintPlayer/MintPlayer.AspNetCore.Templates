using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Repositories;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Action<Options.ApplicationOptions> options)
    {
        var op = new Options.ApplicationOptions(); options(op);

        services
            .AddDbContext<Persistance.MeteoContext>(options => options.UseSqlServer(op.ConnectionString))
            .AddScoped<IAccountRepository, AccountRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IRoleService, RoleService>();

        services
            .AddIdentity<Persistance.Entities.User, Persistance.Entities.Role>()
            .AddEntityFrameworkStores<Persistance.MeteoContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
