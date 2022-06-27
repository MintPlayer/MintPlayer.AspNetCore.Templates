using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Extensions;
using MintPlayer.AspNetCore.SubDirectoryViews;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Web;

public class Startup
{
	public Startup(IConfiguration configuration, IHostEnvironment environment)
	{
		Configuration = configuration;
		Environment = environment;
	}

	public IConfiguration Configuration { get; }
	public IHostEnvironment Environment { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	public void ConfigureServices(IServiceCollection services)
	{
		services
			.ConfigureViewsInSubfolder("Server")
			.AddControllersWithViews()
			.AddNewtonsoftJson();

		services.AddSso(options =>
		{
			options.ConnectionString = Configuration.GetConnectionString("Application");
			options.Environment = Environment;
		});
	}
}