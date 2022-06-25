using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Extensions;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web;

public class Startup
{
	public Startup(IConfiguration configuration, IHostEnvironment environment)
	{
		Configuration = configuration;
		Environment = environment;
	}

	public IConfiguration Configuration { get; init; }
	public IHostEnvironment Environment { get; init; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddControllersWithViews().AddNewtonsoftJson();

		services.AddSso(options =>
		{
			options.ConnectionString = Configuration.GetConnectionString("Sso");
			options.Environment = Environment;
		});
	}
}