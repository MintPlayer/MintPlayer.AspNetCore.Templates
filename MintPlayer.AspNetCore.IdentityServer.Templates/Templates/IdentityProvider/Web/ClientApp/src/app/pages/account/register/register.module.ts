import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { BsAlertModule, BsForModule } from '@mintplayer/ng-bootstrap';

import { RegisterRoutingModule } from './register-routing.module';
import { RegisterComponent } from './register.component';


@NgModule({
  declarations: [
    RegisterComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    BsAlertModule,
    BsForModule,
    RegisterRoutingModule
  ]
})
export class RegisterModule { }
