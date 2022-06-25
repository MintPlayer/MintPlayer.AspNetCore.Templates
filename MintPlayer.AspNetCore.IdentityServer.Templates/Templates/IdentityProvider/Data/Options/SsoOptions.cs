using Microsoft.Extensions.Hosting;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Options;

public class SsoOptions
{
    internal SsoOptions() { }

    public string ConnectionString { get; set; } = null!;
    public IHostEnvironment Environment { get; set; } = null!;
}
