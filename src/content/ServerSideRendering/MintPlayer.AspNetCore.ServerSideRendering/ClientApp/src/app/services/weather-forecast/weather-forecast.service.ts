import { APP_BASE_HREF } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Optional } from '@angular/core';
import { WeatherForecast } from '../../interfaces/weather-forecast';
import { of } from 'rxjs';

@Injectable({
	providedIn: 'root'
})
export class WeatherForecastService {

	constructor(@Inject(APP_BASE_HREF) private baseUrl: string, @Optional() private httpClient?: HttpClient) { }

	public getWeatherForecasts() {
		if (this.httpClient) {
			return this.httpClient.get<WeatherForecast[]>(`${this.baseUrl}/WeatherForecast`);
		} else {
			return of([]);
		}
	}

}
