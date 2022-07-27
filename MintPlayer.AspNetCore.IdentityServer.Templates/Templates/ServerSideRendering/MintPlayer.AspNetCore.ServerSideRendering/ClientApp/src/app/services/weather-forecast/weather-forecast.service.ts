import { APP_BASE_HREF } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { WeatherForecast } from '../../interfaces/weather-forecast';

@Injectable({
	providedIn: 'root'
})
export class WeatherForecastService {

	constructor(private httpClient: HttpClient, @Inject(APP_BASE_HREF) private baseUrl: string) { }

	public getWeatherForecasts() {
		return this.httpClient.get<WeatherForecast[]>(`${this.baseUrl}/WeatherForecast`);
	}

}
