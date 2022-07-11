namespace MintPlayer.AspNetCore.ServerSideRendering.Services;

public interface IWeatherForecastService
{
    Task<IEnumerable<WeatherForecast>> GetWeatherForecasts();
}

public class WeatherForecastService : IWeatherForecastService
{
    private readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public Task<IEnumerable<WeatherForecast>> GetWeatherForecasts()
    {
        var weatherForecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        })
        .ToArray();

        return Task.FromResult<IEnumerable<WeatherForecast>>(weatherForecasts);
    }
}
