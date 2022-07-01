using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.NoSniff;
using MintPlayer.AspNetCore.SubDirectoryViews;
using MintPlayer.AspNetCore.XsrfForSpas;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Extensions;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Web.Services;
using Microsoft.AspNetCore.Http;

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Web;

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

		services.AddDataProtection();
		services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

		services.AddSso(options =>
		{
			options.ConnectionString = Configuration.GetConnectionString("Sso");
			options.Environment = Environment;
		});
		services.AddScoped<IMailService, MailService>();

		// In production, the Angular files will be served from this directory
		services.AddSpaStaticFiles(configuration =>
		{
			configuration.RootPath = "ClientApp/dist/ClientApp";
		});

		services
			.Configure<IdentityOptions>(options =>
			{
				options.SignIn.RequireConfirmedEmail = Configuration.GetValue<bool>("Account:RequireEmailConfirmation", true);
			})
			.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.SlidingExpiration = true;
				options.ExpireTimeSpan = TimeSpan.FromDays(3);
				options.Events.OnRedirectToLogin = (context) =>
				{
					context.Response.StatusCode = StatusCodes.Status401Unauthorized;
					return Task.CompletedTask;
				};
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
		app.UseIdentityServer();
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