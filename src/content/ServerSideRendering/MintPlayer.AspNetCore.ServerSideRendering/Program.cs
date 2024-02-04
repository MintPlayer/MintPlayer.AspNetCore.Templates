using MintPlayer.AspNetCore.Hsts;
using MintPlayer.AspNetCore.SpaServices.Extensions;
using MintPlayer.AspNetCore.SpaServices.Prerendering;
using MintPlayer.AspNetCore.SpaServices.Routing;
using MintPlayer.AspNetCore.SubDirectoryViews;
using System.Text.RegularExpressions;
using WebMarkupMin.AspNetCore8;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddSpaStaticFilesImproved(options =>
{
	options.RootPath = "ClientApp/dist/browser";
});
builder.Services.AddSpaPrerenderingService<WebApplication55.Services.SpaPrerenderingService>();
builder.Services.AddWebMarkupMin(options =>
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
	options.MinificationSettings.MinifyEmbeddedCssCode = true;
	options.MinificationSettings.MinifyInlineCssCode = true;
	options.MinificationSettings.WhitespaceMinificationMode = WebMarkupMin.Core.WhitespaceMinificationMode.Aggressive;
});
builder.Services.AddScoped<WebApplication55.Services.IWeatherForecastService, WebApplication55.Services.WeatherForecastService>();
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
app.UseWebMarkupMin();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller}/{action=Index}/{id?}");
});

if (!app.Environment.IsDevelopment())
{
	app.UseSpaStaticFilesImproved();
}

app.UseSpaImproved(spa =>
{
	spa.Options.SourcePath = "ClientApp";
	// For angular 17
	spa.Options.CliRegexes = [new Regex(@"Local\:\s+(?<openbrowser>https?\:\/\/(.+))")];

	spa.UseSpaPrerendering(options =>
	{
		// Disable below line, and run "npm run build:ssr" or "npm run dev:ssr" manually for faster development.
		options.BootModuleBuilder = app.Environment.IsDevelopment() ? new AngularPrerendererBuilder(npmScript: "build:ssr", @"Build at\:", 1) : null;
		options.BootModulePath = $"{spa.Options.SourcePath}/dist/browser/server/main.js";
		options.ExcludeUrls = new[] { "/sockjs-node" };
	});

	app.UseWebMarkupMin();

	if (app.Environment.IsDevelopment())
	{
		spa.UseAngularCliServer(npmScript: "start");
	}
});

app.Run();
