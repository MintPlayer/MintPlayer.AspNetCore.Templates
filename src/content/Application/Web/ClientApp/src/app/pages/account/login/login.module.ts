import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsAlertModule } from '@mintplayer/ng-bootstrap/alert';

import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { ExternalLoginModule } from '../../../directives/external-login/external-login.module';


@NgModule({
	declarations: [
		LoginComponent
	],
	imports: [
		CommonModule,
		BsAlertModule,
		ExternalLoginModule,
		LoginRoutingModule
	]
})
export class LoginModule { }
