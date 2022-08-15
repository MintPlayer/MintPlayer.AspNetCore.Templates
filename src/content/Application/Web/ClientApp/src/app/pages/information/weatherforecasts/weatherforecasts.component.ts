import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { WeatherForecast } from '../../../api/dtos/weather-forecast';
import { WeatherforecastService } from '../../../api/services/weatherforecast/weatherforecast.service';

@Component({
	selector: 'app-weatherforecasts',
	templateUrl: './weatherforecasts.component.html',
	styleUrls: ['./weatherforecasts.component.scss']
})
export class WeatherforecastsComponent implements OnInit {

	constructor(private weatherForecastService: WeatherforecastService) { }

	weatherForecasts$ = new BehaviorSubject<WeatherForecast[]>([]);

	ngOnInit() {
		this.weatherForecastService.getWeatherForecasts().subscribe({
			next: (weatherForecasts) => this.weatherForecasts$.next(weatherForecasts),
			error: (error) => console.error(error)
		});
	}

}
