import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsTableModule } from '@mintplayer/ng-bootstrap/table';

import { WeatherforecastsRoutingModule } from './weatherforecasts-routing.module';
import { WeatherforecastsComponent } from './weatherforecasts.component';


@NgModule({
	declarations: [
		WeatherforecastsComponent
	],
	imports: [
		CommonModule,
		BsTableModule,
		WeatherforecastsRoutingModule
	]
})
export class WeatherforecastsModule { }
