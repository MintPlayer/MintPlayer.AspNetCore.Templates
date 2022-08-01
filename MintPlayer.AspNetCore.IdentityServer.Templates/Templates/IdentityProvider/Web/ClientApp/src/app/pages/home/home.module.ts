import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsAlertModule } from '@mintplayer/ng-bootstrap';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';


@NgModule({
	declarations: [
		HomeComponent
	],
	imports: [
		CommonModule,
		BsAlertModule,
		HomeRoutingModule
	]
})
export class HomeModule { }
