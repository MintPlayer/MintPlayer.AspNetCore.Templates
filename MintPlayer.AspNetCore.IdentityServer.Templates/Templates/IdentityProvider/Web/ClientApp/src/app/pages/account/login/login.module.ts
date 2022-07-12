import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AdvancedRouterModule } from '@mintplayer/ng-router';
import { BsAlertModule, BsForModule } from '@mintplayer/ng-bootstrap';


import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';


@NgModule({
	declarations: [
		LoginComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		BsForModule,
		BsAlertModule,
		AdvancedRouterModule,
		LoginRoutingModule
	]
})
export class LoginModule { }
