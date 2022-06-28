using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Extensions;
using MintPlayer.AspNetCore.NoSniff;
using MintPlayer.AspNetCore.SubDirectoryViews;
using MintPlayer.AspNetCore.XsrfForSpas;

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

		services.AddApplication(options =>
		{
			options.ConnectionString = Configuration.GetConnectionString("Application");
			options.Environment = Environment;
		});

		services.AddDataProtection();
		services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

		// In production, the Angular files will be served from this directory
		services.AddSpaStaticFiles(configuration =>
		{
			configuration.RootPath = "ClientApp/dist/ClientApp";
		});
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
		}

		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseImprovedHsts();
		app.UseHttpsRedirection();
		app.UseNoSniff();
		app.UseAntiforgery();
		app.UseStaticFiles();

		app.UseAuthentication();
		app.UseRouting();
		app.UseAuthorization();
		app.UseEndpoints(endpoints =>
		{
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller}/{action=Index}/{id?}");
		});

		if (!env.IsDevelopment())
		{
			app.UseSpaStaticFiles();
		}

		app.UseSpa(spa =>
		{
			spa.Options.SourcePath = "ClientApp";

			if (env.IsDevelopment())
			{
				spa.UseAngularCliServer(npmScript: "start");
			}
		});
	}
}