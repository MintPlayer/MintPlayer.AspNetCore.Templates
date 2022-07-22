import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ExternalLoginProviderInfo } from '../../../api/dtos/external-login-provider-info';
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

	@Input() public provider!: ExternalLoginProviderInfo;
	@Input() public action: 'add' | 'connect' = 'connect';
	@Output() public loginSuccessOrFailed = new EventEmitter<ExternalLoginResult>();

	onLoginSuccessOrFailed(ev: ExternalLoginResult) {
		this.loginSuccessOrFailed.emit(ev);
	}

}
