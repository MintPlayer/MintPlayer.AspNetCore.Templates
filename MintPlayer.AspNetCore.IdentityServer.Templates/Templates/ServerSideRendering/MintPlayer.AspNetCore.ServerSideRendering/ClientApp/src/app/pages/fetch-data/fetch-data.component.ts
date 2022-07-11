import { isPlatformServer } from '@angular/common';
import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { DATA_FROM_SERVER } from '../../providers/data-from-server';
import { WeatherForecast } from '../../interfaces/weather-forecast'
import { WeatherForecastService } from '../../services/weather-forecast/weather-forecast.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.scss']
})
export class FetchDataComponent {
  forecasts: WeatherForecast[] = [];

  constructor(
    @Inject(PLATFORM_ID) platformId: Object,
    private weatherForecastService: WeatherForecastService,
    @Inject(DATA_FROM_SERVER) dataFromServer: { weatherForecasts: WeatherForecast[] }
  ) {
    if (isPlatformServer(platformId)) {
      this.forecasts = dataFromServer.weatherForecasts;
    } else {
      this.weatherForecastService.getWeatherForecasts().subscribe(result => {
        this.forecasts = result;
      }, error => {
        console.error(error);
      });
    }
  }
}
