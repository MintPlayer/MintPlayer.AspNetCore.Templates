using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.NoSniff;
using MintPlayer.AspNetCore.SubDirectoryViews;
using MintPlayer.AspNetCore.XsrfForSpas;
#if (UseServerSideRendering)
using MintPlayer.AspNetCore.SpaServices.Prerendering;
using MintPlayer.AspNetCore.SpaServices.Routing;
#endif
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Extensions;
using MintPlayer.AspNetCore.IdentityServer.Application.Web.Extensions;
#if (UseHtmlMinification)
using WebMarkupMin.AspNetCore7;
#endif

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
			.AddControllersWithViews()
			.AddNewtonsoftJson();

		services.ConfigureViewsInSubfolder("Server");

		var authenticationBuilder = services
			.AddAuthentication();

		if (Configuration.TryGetValue("Authentication:Central", out OpenIdConnectOptions central))
		{
			if (!string.IsNullOrEmpty(central.ClientId) && !string.IsNullOrEmpty(central.ClientSecret))
			{
				authenticationBuilder.AddOpenIdConnect("central", options =>
				{
					options.ClientId = central.ClientId;
					options.ClientSecret = central.ClientSecret;
					options.ResponseType = Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectResponseType.Code;
					options.UsePkce = true;
					options.CallbackPath = new PathString("/signin-central");
					options.Authority = central.Authority;
				
					options.Scope.Add("openid");
					options.Scope.Add("profile");
					options.Scope.Add("email");
				});
			}
		}

		services.AddApplication(options =>
		{
			options.ConnectionString = Configuration.GetConnectionString("MintPlayer.AspNetCore.IdentityServer.Application");
			options.Environment = Environment;
		});

#if (UseXsrfProtection)
		services.AddDataProtection();
		services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");
#endif
#if (UseServerSideRendering)
		services.AddSpaPrerenderingService<MintPlayer.AspNetCore.IdentityServer.Application.Web.Services.SpaPrerenderingService>();
#endif
#if (UseHtmlMinification)
		services.AddWebMarkupMin(options =>
		{
			options.DisablePoweredByHttpHeaders = true;
			options.AllowMinificationInDevelopmentEnvironment = true;
			options.AllowCompressionInDevelopmentEnvironment = true;
			options.DisablePoweredByHttpHeaders = false;
		}).AddHttpCompression(options =>
		{
		}).AddHtmlMinification(options =>
		{
			options.MinificationSettings.RemoveEmptyAttributes = true;
			options.MinificationSettings.RemoveRedundantAttributes = true;
			options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
			options.MinificationSettings.RemoveHttpsProtocolFromAttributes = false;
			options.MinificationSettings.MinifyInlineJsCode = true;
			options.MinificationSettings.MinifyEmbeddedJsCode = true;
			options.MinificationSettings.MinifyEmbeddedJsonData = true;
			options.MinificationSettings.WhitespaceMinificationMode = WebMarkupMin.Core.WhitespaceMinificationMode.Aggressive;
		});
#endif

		// In production, the Angular files will be served from this directory
		services.AddSpaStaticFiles(configuration =>
		{
			configuration.RootPath = "ClientApp/dist";
		});
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.ApplicationServices.GetRequiredService<IDatabaseService>().Migrate();
		}
		else
		{
			app.UseExceptionHandler("/Error");
		}

		// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		app.UseImprovedHsts();
		app.UseHttpsRedirection();
		app.UseNoSniff();
#if (UseXsrfProtection)
		app.UseAntiforgery();
#endif
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

#if (UseServerSideRendering)
			spa.UseSpaPrerendering(options =>
			{
				// Disable below line, and run "npm run build:ssr" or "npm run dev:ssr" manually for faster development.
				options.BootModuleBuilder = env.IsDevelopment() ? new AngularPrerendererBuilder(npmScript: "build:ssr") : null!;
				options.BootModulePath = $"{spa.Options.SourcePath}/dist/ClientApp/server/main.js";
				options.ExcludeUrls = new[] { "/sockjs-node" };
			});

#endif
#if (UseHtmlMinification)
			app.UseWebMarkupMin();

#endif
			if (env.IsDevelopment())
			{
				spa.UseAngularCliServer(npmScript: "start");
			}
		});
	}
}
