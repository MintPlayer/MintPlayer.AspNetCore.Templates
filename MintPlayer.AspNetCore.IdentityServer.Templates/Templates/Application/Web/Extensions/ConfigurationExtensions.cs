using Microsoft.Extensions.Configuration;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Web.Extensions;

internal static class ConfigurationExtensions
{
	public static bool TryGetValue<T>(this IConfiguration configuration, string key, out T value)
	{
		var config = Activator.CreateInstance<T>();
		configuration.GetSection(key).Bind(config);

		if (config == null)
		{
			value = default!;
			return false;
		}
		else
		{
			value = config;
			return true;
		}
	}
}
