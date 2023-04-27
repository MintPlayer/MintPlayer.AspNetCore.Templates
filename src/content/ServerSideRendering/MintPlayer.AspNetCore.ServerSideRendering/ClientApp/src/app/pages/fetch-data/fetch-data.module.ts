import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsTableModule } from '@mintplayer/ng-bootstrap/table';

import { FetchDataRoutingModule } from './fetch-data-routing.module';
import { FetchDataComponent } from './fetch-data.component';


@NgModule({
	declarations: [
		FetchDataComponent
	],
	imports: [
		CommonModule,
		BsTableModule,
		FetchDataRoutingModule
	]
})
export class FetchDataModule { }
