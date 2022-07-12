import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { A11yModule } from '@angular/cdk/a11y';
import { BsAlertModule, BsForModule, BsModalModule } from '@mintplayer/ng-bootstrap';
import { FocusOnLoadModule } from '@mintplayer/ng-focus-on-load';

import { TwoFactorRoutingModule } from './two-factor-routing.module';
import { TwoFactorComponent } from './two-factor.component';


@NgModule({
	declarations: [
		TwoFactorComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		BsForModule,
		BsAlertModule,
		BsModalModule,
		FocusOnLoadModule,
		A11yModule,
		TwoFactorRoutingModule
	]
})
export class TwoFactorModule { }
