using Microsoft.Extensions.Hosting;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Options;

public class ApplicationOptions
{
    internal ApplicationOptions() { }

    public string ConnectionString { get; set; } = null!;
    public IHostEnvironment Environment { get; set; } = null!;
}
