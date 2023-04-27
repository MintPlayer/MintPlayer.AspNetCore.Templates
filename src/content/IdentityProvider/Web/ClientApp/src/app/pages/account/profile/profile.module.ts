import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { A11yModule } from '@angular/cdk/a11y';
import { QrCodeModule } from '@mintplayer/ng-qr-code';
import { BsCardModule } from '@mintplayer/ng-bootstrap/card';
import { BsGridModule } from '@mintplayer/ng-bootstrap/grid';
import { BsForModule } from '@mintplayer/ng-bootstrap/for';
import { BsFormModule } from '@mintplayer/ng-bootstrap/form';
import { BsModalModule } from '@mintplayer/ng-bootstrap/modal';
import { BsListGroupModule } from '@mintplayer/ng-bootstrap/list-group';
import { BsButtonGroupModule } from '@mintplayer/ng-bootstrap/button-group';
import { BsButtonTypeModule } from '@mintplayer/ng-bootstrap/button-type';
import { BsInputGroupModule } from '@mintplayer/ng-bootstrap/input-group';
import { FocusOnLoadModule } from '@mintplayer/ng-focus-on-load';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile.component';
//#if (UseExternalLogins)
import { ExternalLoginModule } from '../../../directives/external-login/external-login.module';
//#endif


@NgModule({
	declarations: [
		ProfileComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		BsForModule,
		BsFormModule,
		BsCardModule,
		BsGridModule,
		BsModalModule,
		BsListGroupModule,
		BsButtonGroupModule,
		BsButtonTypeModule,
		BsInputGroupModule,
		QrCodeModule,
		A11yModule,
		FocusOnLoadModule,
//#if (UseExternalLogins)
		ExternalLoginModule,
//#endif
		ProfileRoutingModule
	]
})
export class ProfileModule { }
