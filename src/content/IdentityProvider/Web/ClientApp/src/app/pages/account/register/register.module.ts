import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BsAlertModule } from '@mintplayer/ng-bootstrap/alert';
import { BsForModule } from '@mintplayer/ng-bootstrap/for';
import { BsFormModule } from '@mintplayer/ng-bootstrap/form';
import { BsGridModule } from '@mintplayer/ng-bootstrap/grid';
import { BsButtonTypeModule } from '@mintplayer/ng-bootstrap/button-type';

import { RegisterRoutingModule } from './register-routing.module';
import { RegisterComponent } from './register.component';


@NgModule({
	declarations: [
		RegisterComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		BsAlertModule,
		BsForModule,
		BsFormModule,
		BsGridModule,
		BsButtonTypeModule,
		RegisterRoutingModule
	]
})
export class RegisterModule { }
