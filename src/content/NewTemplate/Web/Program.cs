using System.Text.RegularExpressions;
using MintPlayer.AspNetCore.SpaServices.Extensions;
using MintPlayer.AspNetCore.SpaServices.Routing;

namespace MintPlayer.AspNetCore.Template.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();
        builder.Services.AddSpaStaticFilesImproved(configuration => {
            // In production, the Angular files will be served from this directory
            configuration.RootPath = "ClientApp/dist";
        });
        builder.Services.AddSpaPrerenderingService<MintPlayer.AspNetCore.Template.Services.SpaPrerenderingService>();

        var app = builder.Build();
        app.UseStaticFiles();
        if (!app.Environment.IsDevelopment()) {
            app.UseSpaStaticFilesImproved();
        }
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
        app.UseSpaImproved(spa => {
            spa.Options.SourcePath = "ClientApp";
            // For angular 17
            spa.Options.CliRegexes = [new Regex(@"Local\:\s+(?<openbrowser>https?\:\/\/(.+))")];
            if (app.Environment.IsDevelopment()) {
                spa.UseAngularCliServer(npmScript: "start");
            }
        });
        app.Run();
    }
}