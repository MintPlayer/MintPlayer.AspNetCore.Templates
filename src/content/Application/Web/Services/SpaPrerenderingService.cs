using MintPlayer.AspNetCore.SpaServices.Routing;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Web.Services;

public class SpaPrerenderingService : ISpaPrerenderingService
{
	#region Constructor
	private readonly ISpaRouteService spaRouteService;
	private readonly IWeatherForecastService weatherForecastService;
	public SpaPrerenderingService(ISpaRouteService spaRouteService, IWeatherForecastService weatherForecastService)
	{
		this.spaRouteService = spaRouteService;
		this.weatherForecastService = weatherForecastService;
	}
	#endregion

	public Task BuildRoutes(ISpaRouteBuilder routeBuilder)
	{
		routeBuilder
			.Route("", "home")
			.Route("counter", "counter")
			.Route("fetch-data", "fetchdata");

		return Task.CompletedTask;
	}

	public async Task OnSupplyData(HttpContext context, IDictionary<string, object> data)
	{
		var route = await spaRouteService.GetCurrentRoute(context);
		switch (route?.Name)
		{
			case "fetchdata":
				var weatherForecasts = await weatherForecastService.GetWeatherForecasts();
				data["weatherForecasts"] = weatherForecasts;
				break;
			default:
				break;
		}
	}
}
