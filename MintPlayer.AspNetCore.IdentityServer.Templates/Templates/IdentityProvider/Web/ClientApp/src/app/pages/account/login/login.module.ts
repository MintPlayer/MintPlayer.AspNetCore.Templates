import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AdvancedRouterModule } from '@mintplayer/ng-router';
import { BsAlertModule, BsForModule } from '@mintplayer/ng-bootstrap';


import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { ExternalLoginModule } from '../../../directives/external-login/external-login.module';


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
		ExternalLoginModule,
		LoginRoutingModule
	]
})
export class LoginModule { }
