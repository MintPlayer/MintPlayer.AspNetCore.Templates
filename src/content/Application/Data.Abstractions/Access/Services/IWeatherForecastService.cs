using MintPlayer.AspNetCore.IdentityServer.Application.Dtos.Dtos;

namespace MintPlayer.AspNetCore.IdentityServer.Application.Data.Abstractions.Access.Services;

public interface IWeatherForecastService
{
	Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
}
