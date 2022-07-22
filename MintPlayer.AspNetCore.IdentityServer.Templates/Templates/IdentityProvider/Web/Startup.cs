using Microsoft.AspNetCore.Authentication.Facebook;
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
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Data.Abstractions.Services;
using MintPlayer.AspNetCore.IdentityServer.Provider.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using MintPlayer.AspNetCore.IdentityServer.Templates.Templates.IdentityProvider.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Authentication.Google;
using AspNet.Security.OAuth.LinkedIn;

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
            .AddControllersWithViews()
            .AddNewtonsoftJson();

        services.ConfigureViewsInSubfolder("Server");
        services.AddDataProtection();
        services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

        var authenticationBuilder = services.AddAuthentication();
        if (Configuration.TryGetValue("Authentication:Microsoft", out MicrosoftAccountOptions ms))
        {
            if (!string.IsNullOrEmpty(ms.ClientId) && !string.IsNullOrEmpty(ms.ClientSecret))
            {
                authenticationBuilder.AddMicrosoftAccount(options =>
                {
                    options.ClientId = ms.ClientId;
                    options.ClientSecret = ms.ClientSecret;
                });
            }
        }
        if (Configuration.TryGetValue("Authentication:Google", out GoogleOptions g))
        {
            if (!string.IsNullOrEmpty(g.ClientId) && !string.IsNullOrEmpty(g.ClientSecret))
            {
                authenticationBuilder.AddGoogle(options =>
                {
                    options.ClientId = g.ClientId;
                    options.ClientSecret = g.ClientSecret;
                });
            }
        }
        if (Configuration.TryGetValue("Authentication:Linkedin", out LinkedInAuthenticationOptions li))
        {
            if (!string.IsNullOrEmpty(li.ClientId) && !string.IsNullOrEmpty(li.ClientSecret))
            {
                authenticationBuilder.AddLinkedIn(options =>
                {
                    options.ClientId = li.ClientId;
                    options.ClientSecret = li.ClientSecret;
                    options.ReturnUrlParameter = "/signin-linkedin";
                });
            }
        }
        if (Configuration.TryGetValue("Authentication:Facebook", out FacebookOptions fb))
        {
            if (!string.IsNullOrEmpty(fb.ClientId) && !string.IsNullOrEmpty(fb.ClientSecret))
            {
                authenticationBuilder.AddFacebook(options =>
                {
                    options.AppId = fb.AppId;
                    options.AppSecret = fb.AppSecret;
                });
            }
        }
        if (Configuration.TryGetValue("Authentication:Twitter", out TwitterOptions tw))
        {
            if (!string.IsNullOrEmpty(tw.ConsumerKey) && !string.IsNullOrEmpty(tw.ConsumerSecret))
            {
                authenticationBuilder.AddTwitter(options =>
                {
                    options.ConsumerKey = tw.ConsumerKey;
                    options.ConsumerSecret = tw.ConsumerSecret;
                    options.RetrieveUserDetails = true;
                });
            }
        }

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