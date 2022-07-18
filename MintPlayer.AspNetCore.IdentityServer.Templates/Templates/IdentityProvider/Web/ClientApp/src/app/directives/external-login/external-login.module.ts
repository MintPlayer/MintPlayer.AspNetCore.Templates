import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExternalLoginDirective } from './external-login.directive';



@NgModule({
  declarations: [
    ExternalLoginDirective
  ],
  imports: [
    CommonModule
  ]
})
export class ExternalLoginModule { }
