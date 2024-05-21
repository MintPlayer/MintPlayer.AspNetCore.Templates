using MintPlayer.AspNetCore.SpaServices.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSpaStaticFilesImproved(configuration => {
    // In production, the Angular files will be served from this directory
    configuration.RootPath = "ClientApp/dist";
});
builder.Services.AddSpaPrerenderingService<MintPlayer.AspNetCore.Template.Services.SpaPrerenderingService>();

var app = builder.Build();
app.UseStaticFiles();
if (!env.IsDevelopment()) {
    app.Environment.UseSpaStaticFilesImproved();
}
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller}/{action=Index}/{id?}");
app.UseSpaImproved(spa => {
    spa.Options.SourcePath = "ClientApp";
    // For angular 17
    spa.Options.CliRegexes = [new Regex(@"Local\:\s+(?<openbrowser>https?\:\/\/(.+))")];
    if (env.IsDevelopment()) {
        spa.UseAngularCliServer(npmScript: "start");
    }
});
app.Run();