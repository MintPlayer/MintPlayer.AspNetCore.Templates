import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthenticationScheme } from '../../../api/dtos/authentication-scheme';
import { ExternalLoginResult } from '../../../api/dtos/external-login-result';

@Component({
	selector: 'external-login-button',
	templateUrl: './external-login-button.component.html',
	styleUrls: ['./external-login-button.component.scss']
})
export class ExternalLoginButtonComponent implements OnInit {

	constructor() {
	}

	ngOnInit() {
	}

	@Input() public provider!: AuthenticationScheme;
	@Input() public action: 'add' | 'connect' = 'connect';
	@Output() public loginSuccessOrFailed = new EventEmitter<ExternalLoginResult>();

	onLoginSuccessOrFailed(ev: ExternalLoginResult) {
		this.loginSuccessOrFailed.emit(ev);
	}

}
