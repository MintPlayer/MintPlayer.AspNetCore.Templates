import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { A11yModule } from '@angular/cdk/a11y';
import { QrCodeModule } from '@mintplayer/ng-qr-code';
import { BsCardModule, BsForModule, BsModalModule } from '@mintplayer/ng-bootstrap';
import { FocusOnLoadModule } from '@mintplayer/ng-focus-on-load';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile.component';


@NgModule({
	declarations: [
		ProfileComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		BsForModule,
		BsCardModule,
		BsModalModule,
		QrCodeModule,
		A11yModule,
		FocusOnLoadModule,
		ProfileRoutingModule
	]
})
export class ProfileModule { }
