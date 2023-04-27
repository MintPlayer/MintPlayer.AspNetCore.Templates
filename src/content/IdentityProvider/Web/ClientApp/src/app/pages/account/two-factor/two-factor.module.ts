import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { A11yModule } from '@angular/cdk/a11y';
import { BsAlertModule } from '@mintplayer/ng-bootstrap/alert';
import { BsForModule } from '@mintplayer/ng-bootstrap/for';
import { BsFormModule } from '@mintplayer/ng-bootstrap/form';
import { BsModalModule } from '@mintplayer/ng-bootstrap/modal';
import { BsGridModule } from '@mintplayer/ng-bootstrap/grid';
import { BsButtonGroupModule } from '@mintplayer/ng-bootstrap/button-group';
import { BsButtonTypeModule } from '@mintplayer/ng-bootstrap/button-type';
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
		BsFormModule,
		BsAlertModule,
		BsModalModule,
		BsGridModule,
		BsButtonGroupModule,
		BsButtonTypeModule,
		FocusOnLoadModule,
		A11yModule,
		TwoFactorRoutingModule
	]
})
export class TwoFactorModule { }
