using MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;
using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Access.Services;

internal class WeatherForecastService : IWeatherForecastService
{
	public Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
	{
		var summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
		{
			Date = DateTime.Now.AddDays(index),
			TemperatureC = System.Random.Shared.Next(-20, 55),
			Summary = summaries[System.Random.Shared.Next(summaries.Length)]
		});

		return Task.FromResult(forecasts);
	}
}
