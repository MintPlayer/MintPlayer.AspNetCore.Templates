using Microsoft.AspNetCore.SpaServices.AngularCli;
using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.SpaServices.Prerendering;
using MintPlayer.AspNetCore.SpaServices.Routing;
using MintPlayer.AspNetCore.SubDirectoryViews;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSpaStaticFiles(options =>
{
    options.RootPath = "ClientApp/dist";
});
builder.Services.AddSpaPrerenderingService<MintPlayer.AspNetCore.ServerSideRendering.Services.SpaPrerenderingService>();
builder.Services.AddScoped<MintPlayer.AspNetCore.ServerSideRendering.Services.IWeatherForecastService, MintPlayer.AspNetCore.ServerSideRendering.Services.WeatherForecastService>();
builder.Services.ConfigureViewsInSubfolder("Server");

var app = builder.Build();

if (app.Environment.IsDevelopment())
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
app.UseStaticFiles();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

if (!app.Environment.IsDevelopment())
{
    app.UseSpaStaticFiles();
}

app.UseSpa(spa =>
{
	spa.Options.SourcePath = "ClientApp";

	spa.UseSpaPrerendering(options =>
	{
        // Disable below line, and run "npm run build:ssr" or "npm run dev:ssr" manually for faster development.
        options.BootModuleBuilder = app.Environment.IsDevelopment() ? new AngularPrerendererBuilder(npmScript: "build:ssr") : null;
        options.BootModulePath = $"{spa.Options.SourcePath}/dist/ClientApp/server/main.js";
		options.ExcludeUrls = new[] { "/sockjs-node" };
	});

	if (app.Environment.IsDevelopment())
	{
		spa.UseAngularCliServer(npmScript: "start");
	}
});

app.Run();
