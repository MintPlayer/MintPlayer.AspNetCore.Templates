using MintPlayer.AspNetCore.SpaServices.Prerendering.Services;
using MintPlayer.AspNetCore.SpaServices.Routing;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Web.Services;

public class SpaPrerenderingService : ISpaPrerenderingService
{
	#region Constructor
	private readonly ISpaRouteService spaRouteService;
	private readonly IWeatherForecastService weatherForecastService;
	private readonly IAccountService accountService;
	public SpaPrerenderingService(ISpaRouteService spaRouteService, IWeatherForecastService weatherForecastService, IAccountService accountService)
	{
		this.spaRouteService = spaRouteService;
		this.weatherForecastService = weatherForecastService;
		this.accountService = accountService;
	}
	#endregion

	public Task BuildRoutes(ISpaRouteBuilder routeBuilder)
	{
		routeBuilder
			.Route("", "home")
			.Route("counter", "counter")
			.Group("information", "information", routes =>
			{
				routes.Route("weatherforecasts", "weatherforecasts");
			})
			.Group("account", "account", routes =>
			{
				routes.Route("login", "login");
			});

		return Task.CompletedTask;
	}

	public async Task OnSupplyData(HttpContext context, IDictionary<string, object> data)
	{
		var route = await spaRouteService.GetCurrentRoute(context);
		var user = await accountService.GetCurrentUser();
		data["user"] = user;
		switch (route?.Name)
		{
			case "information-weatherforecasts":
				if (user == null)
				{
					var url = await spaRouteService.GenerateUrl("account-login", new { returnUrl = context.Request.Path });
					context.Response.Redirect(url, false);
				}
				else
				{
					var weatherForecasts = await weatherForecastService.GetWeatherForecasts();
					data["weatherForecasts"] = weatherForecasts;
				}
				break;
			default:
				break;
		}
	}
}
