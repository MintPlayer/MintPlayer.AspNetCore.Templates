import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrlService } from '@mintplayer/ng-base-url';
import { WeatherForecast } from '../../dtos/weather-forecast';

@Injectable({
	providedIn: 'root'
})
export class WeatherforecastService {

	constructor(baseUrlService: BaseUrlService, private httpClient: HttpClient) {
		this.baseUrl = baseUrlService.getBaseUrl({ dropScheme: true });
	}

	private baseUrl: string | null;

	public getWeatherForecasts() {
		return this.httpClient.get<WeatherForecast[]>(`${this.baseUrl}/Web/V1/WeatherForecast`);
	}
}
