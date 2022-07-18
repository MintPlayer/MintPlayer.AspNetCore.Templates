using Microsoft.Extensions.Configuration;

namespace MintPlayer.AspNetCore.IdentityServer.Templates.Templates.IdentityProvider.Web.Extensions;

internal static class ConfigurationExtensions
{
    public static bool TryGetValue<T>(this IConfiguration configuration, string key, out T value)
    {
        if (configuration[key] == null)
        {
            value = default!;
            return false;
        }
        else
        {
            value = configuration.GetValue<T>(key, default!);
            return true;
        }
    }
}
