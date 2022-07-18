import { Directive, EventEmitter, HostBinding, HostListener, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { BaseUrlService } from '@mintplayer/ng-base-url';
import { LoginResult } from '../../api/dtos/login-result';

@Directive({
	selector: '[externalLogin]'
})
export class ExternalLoginDirective implements OnInit, OnDestroy {

	constructor(private baseUrlService: BaseUrlService) {
		this.externalUrl = this.baseUrlService.getBaseUrl({ subdomain: 'external', dropScheme: false });
	}

	authWindow: Window | null = null;
	listener?: any;
	externalUrl: string | null;

	@Input() public action: 'add' | 'connect' = 'connect';
	@Input('externalLogin') public provider: string | null = null;
	@Output() public loginSuccessOrFailed = new EventEmitter<LoginResult>();
	@HostBinding('disabled') isOpen = false;

	ngOnInit() {
		this.listener = this.handleMessage.bind(this);
		if (typeof window !== 'undefined') {
			if (window.addEventListener) {
				window.addEventListener('message', this.listener, false);
			} else {
				(<any>window).attachEvent('onmessage', this.listener);
			}
		}
	}

	ngOnDestroy() {
		if (typeof window !== 'undefined') {
			if (window.removeEventListener) {
				window.removeEventListener('message', this.listener, false);
			} else {
				(<any>window).detachEvent('onmessage', this.listener);
			}
		}
	}

	@HostListener('click', ['$event'])
	onClick(ev: MouseEvent) {
		if ((typeof window !== 'undefined') && (this.externalUrl)) {
			this.authWindow = window.open(`${this.externalUrl}/Web/V1/Account/ExternalLogin/Connect/${this.provider}`, '_blank', 'width=600,height=400');
			this.isOpen = true;
			const timer = setInterval(() => {
				if (this.authWindow?.closed) {
					this.isOpen = false;
					clearInterval(timer);
				}
			});
		}
	}

	handleMessage(event: Event) {
		const message = event as MessageEvent;

		// Only trust messages from the below origin.
		const messageOrigin = message.origin.replace(/^https?\:/, '');
		if (this.externalUrl && !this.externalUrl.startsWith(messageOrigin)) return;

		// Filter out Augury
		if (message.data.messageSource != null)
			if (message.data.messageSource.indexOf('AUGURY_') > -1) return;
		// Filter out any other trash
		if (message.data == '' || message.data == null) return;

		const result = <LoginResult>JSON.parse(message.data);
		//var medium = this.pwaHelper.isPwa() ? 'pwa' : 'web';
		if ((result.provider === this.provider) && this.authWindow) {
			this.authWindow.close();
			this.loginSuccessOrFailed.emit(result);
		}
	}

}
