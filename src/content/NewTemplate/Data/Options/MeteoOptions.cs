using Microsoft.Extensions.Hosting;

namespace MintPlayer.AspNetCore.Template.Data.Options;

public class MeteoOptions
{
	internal MeteoOptions() { }

	public string ConnectionString { get; set; } = null!;
	public IHostEnvironment Environment { get; set; } = null!;
}
