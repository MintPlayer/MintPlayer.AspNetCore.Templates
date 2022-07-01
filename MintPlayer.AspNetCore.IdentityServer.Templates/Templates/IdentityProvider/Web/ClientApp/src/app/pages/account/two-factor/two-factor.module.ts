import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BsAlertModule, BsForModule } from '@mintplayer/ng-bootstrap';

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
        TwoFactorRoutingModule
    ]
})
export class TwoFactorModule { }
