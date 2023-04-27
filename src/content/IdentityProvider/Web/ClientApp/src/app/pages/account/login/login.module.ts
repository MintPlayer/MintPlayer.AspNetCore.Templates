import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { A11yModule } from '@angular/cdk/a11y';
import { AdvancedRouterModule } from '@mintplayer/ng-router';
import { BsAlertModule } from '@mintplayer/ng-bootstrap/alert';
import { BsForModule } from '@mintplayer/ng-bootstrap/for';
import { BsFormModule } from '@mintplayer/ng-bootstrap/form';
import { BsGridModule } from '@mintplayer/ng-bootstrap/grid';
import { BsModalModule } from '@mintplayer/ng-bootstrap/modal';
import { BsButtonTypeModule } from '@mintplayer/ng-bootstrap/button-type';
import { BsButtonGroupModule } from '@mintplayer/ng-bootstrap/button-group';


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
		BsFormModule,
		BsGridModule,
		BsAlertModule,
		BsModalModule,
		BsButtonTypeModule,
		BsButtonGroupModule,
		A11yModule,
		AdvancedRouterModule,
//#if (UseExternalLogins)
		ExternalLoginModule,
//#endif
		LoginRoutingModule
	]
})
export class LoginModule { }
