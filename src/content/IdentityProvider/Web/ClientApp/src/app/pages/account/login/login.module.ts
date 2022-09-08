import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { A11yModule } from '@angular/cdk/a11y';
import { AdvancedRouterModule } from '@mintplayer/ng-router';
import { BsAlertModule, BsForModule, BsModalModule } from '@mintplayer/ng-bootstrap';


import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
//#if (UseExternalLogins)
import { ExternalLoginModule } from '../../../directives/external-login/external-login.module';
//#endif


@NgModule({
	declarations: [
		LoginComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		BsForModule,
		BsAlertModule,
		BsModalModule,
		A11yModule,
		AdvancedRouterModule,
//#if (UseExternalLogins)
		ExternalLoginModule,
//#endif
		LoginRoutingModule
	]
})
export class LoginModule { }
