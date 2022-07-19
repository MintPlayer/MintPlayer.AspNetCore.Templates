using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance.Entities;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Persistance;

internal class SsoContext : IdentityDbContext<User, Role, Guid>, IPersistedGrantDbContext, IConfigurationDbContext
{
    #region Constructor
    private readonly IConfiguration? configuration;
    public SsoContext()
    {
        this.configuration = null;
    }
    public SsoContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    #endregion

    public DbSet<PersistedGrant> PersistedGrants { get; set; } = null!;
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; } = null!;
    public DbSet<Key> Keys { get; set; } = null!;
    public DbSet<Client> Clients { get; set; } = null!;
    public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; } = null!;
    public DbSet<IdentityResource> IdentityResources { get; set; } = null!;
    public DbSet<ApiResource> ApiResources { get; set; } = null!;
    public DbSet<ApiScope> ApiScopes { get; set; } = null!;
    public DbSet<Duende.IdentityServer.EntityFramework.Entities.IdentityProvider> IdentityProviders { get; set; } = null!;
    public DbSet<ServerSideSession> ServerSideSessions { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (configuration == null)
        {
            // Only used when generating migrations
            var migrationsConnectionString = @"Server=(localdb)\mssqllocaldb;Database=MintPlayer.AspNetCore.IdentityServer;Trusted_Connection=True;ConnectRetryCount=0";
            optionsBuilder.UseSqlServer(migrationsConnectionString, options => {
                options.MigrationsAssembly(typeof(SsoContext).Assembly.FullName);
            });
        }
        else
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("MintPlayer.AspNetCore.IdentityServer"));
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<DeviceFlowCodes>().HasNoKey();
        builder.Entity<PersistedGrant>().HasKey(pg => pg.Key);
    }

    public async Task<int> SaveChangesAsync()
    {
        var result = await base.SaveChangesAsync();
        return result;
    }
}
