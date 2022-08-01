using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Web.Server.Controllers.Api.V1;

[ApiController]
[Route("Api/V1/[controller]")]
public class WeatherForecastController : Controller
{
	#region Constructor
	private readonly IWeatherForecastService weatherForecastService;
	public WeatherForecastController(IWeatherForecastService weatherForecastService)
	{
		this.weatherForecastService = weatherForecastService;
	}
	#endregion

	[HttpGet]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public async Task<ActionResult<IEnumerable<WeatherForecast>>> GetWeatherForecasts()
	{
		try
		{
			var weatherForecasts = await weatherForecastService.GetWeatherForecasts();
			return Ok(weatherForecasts);
		}
		catch (Exception)
		{
			return StatusCode(500);
		}
	}
}
