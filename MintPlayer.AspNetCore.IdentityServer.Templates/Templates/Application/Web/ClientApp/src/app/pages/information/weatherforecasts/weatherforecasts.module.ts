import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { WeatherforecastsRoutingModule } from './weatherforecasts-routing.module';
import { WeatherforecastsComponent } from './weatherforecasts.component';


@NgModule({
	declarations: [
		WeatherforecastsComponent
	],
	imports: [
		CommonModule,
		WeatherforecastsRoutingModule
	]
})
export class WeatherforecastsModule { }
