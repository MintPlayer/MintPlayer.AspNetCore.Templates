import { isPlatformServer } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { WeatherForecast } from '../../../api/dtos/weather-forecast';
import { WeatherforecastService } from '../../../api/services/weatherforecast/weatherforecast.service';
import { DATA_FROM_SERVER } from '../../../providers/data-from-server';

@Component({
	selector: 'app-weatherforecasts',
	templateUrl: './weatherforecasts.component.html',
	styleUrls: ['./weatherforecasts.component.scss']
})
export class WeatherforecastsComponent implements OnInit {

	constructor(
		@Inject(PLATFORM_ID) private platformId: Object,
		private weatherForecastService: WeatherforecastService,
		@Inject(DATA_FROM_SERVER) private dataFromServer: { weatherForecasts: WeatherForecast[] }) {
	}

	weatherForecasts$ = new BehaviorSubject<WeatherForecast[]>([]);

	ngOnInit() {
		if (isPlatformServer(this.platformId)) {
			this.weatherForecasts$.next(this.dataFromServer.weatherForecasts);
		} else {
			this.weatherForecastService.getWeatherForecasts().subscribe({
				next: (weatherForecasts) => this.weatherForecasts$.next(weatherForecasts),
				error: (error) => console.error(error)
			});
		}
	}
}
