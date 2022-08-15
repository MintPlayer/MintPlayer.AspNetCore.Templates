import { Directive, EventEmitter, HostBinding, HostListener, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { BaseUrlService } from '@mintplayer/ng-base-url';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { ExternalLoginResult } from '../../../api/dtos/external-login-result';

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

	//#region Provider
	provider$ = new BehaviorSubject<string | null>(null);
	public get provider() {
		return this.provider$.value;
	}
	@Input('externalLogin') public set provider(value: string | null) {
		this.provider$.next(value);
	}
	//#endregion

	@Output() public loginSuccessOrFailed = new EventEmitter<ExternalLoginResult>();
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
			console.log('provider', this.provider);
			const url = `${this.externalUrl}/Web/V1/Account/ExternalLogin/${this.action}/${this.provider}`;
			console.log('url', url);
			this.authWindow = window.open(url, '_blank', 'width=600,height=400');
			this.isOpen = true;
			const timer = setInterval(() => {
				if (!this.authWindow || this.authWindow.closed) {
					this.isOpen = false;
					clearInterval(timer);
				}
			});
		}
	}

	handleMessage(event: Event) {
		const message = event as MessageEvent;

		// Only trust messages from the below origin.
		if (this.externalUrl && !this.externalUrl.startsWith(message.origin)) return;

		// Filter out Augury
		if (message.data.messageSource != null)
			if (message.data.messageSource.indexOf('AUGURY_') > -1) return;
		// Filter out any other trash
		if (message.data == '' || message.data == null) return;
		if (message.data.type === 'webpackOk') return;

		const result = <ExternalLoginResult>JSON.parse(message.data);
		//var medium = this.pwaHelper.isPwa() ? 'pwa' : 'web';
		if (result.provider === this.provider) {
			this.authWindow && this.authWindow.close();
			this.loginSuccessOrFailed.emit(result);
		}
	}

}
