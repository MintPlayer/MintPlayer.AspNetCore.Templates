import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExternalLoginDirective } from './directive/external-login.directive';
import { ExternalLoginButtonComponent } from './component/external-login-button.component';



@NgModule({
	declarations: [
		ExternalLoginDirective,
		ExternalLoginButtonComponent
	],
	imports: [
		CommonModule
	],
	exports: [
		ExternalLoginDirective,
		ExternalLoginButtonComponent
	]
})
export class ExternalLoginModule { }
