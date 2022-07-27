import { Component, OnInit } from '@angular/core';
import { WeatherforecastService } from '../../../services/weatherforecast/weatherforecast.service';

@Component({
	selector: 'app-weatherforecasts',
	templateUrl: './weatherforecasts.component.html',
	styleUrls: ['./weatherforecasts.component.scss']
})
export class WeatherforecastsComponent implements OnInit {

	constructor(private weatherForecastService: WeatherforecastService) { }

	ngOnInit(): void {
	}

}
