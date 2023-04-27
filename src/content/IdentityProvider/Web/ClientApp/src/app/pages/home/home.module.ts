import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsGridModule } from '@mintplayer/ng-bootstrap/grid';
import { BsAlertModule } from '@mintplayer/ng-bootstrap/alert';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';


@NgModule({
	declarations: [
		HomeComponent
	],
	imports: [
		CommonModule,
		BsGridModule,
		BsAlertModule,
		HomeRoutingModule
	]
})
export class HomeModule { }
