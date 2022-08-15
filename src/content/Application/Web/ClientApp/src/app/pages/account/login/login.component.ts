import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Color } from '@mintplayer/ng-bootstrap';
import { BehaviorSubject, Subject, takeUntil, tap } from 'rxjs';
import { Store } from '@ngxs/store';
import { AuthenticationScheme } from '../../../api/dtos/authentication-scheme';
import { ExternalLoginResult } from '../../../api/dtos/external-login-result';
import { ELoginStatus } from '../../../api/enums/login-status';
import { AccountService } from '../../../api/services/account/account.service';
import { ErrorMessage } from '../../../entities/error-message';
import { AdvancedRouter } from '@mintplayer/ng-router';
import { SetUser } from '../../../states/application/actions/set-user';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {

	constructor(private accountService: AccountService, private router: AdvancedRouter, private route: ActivatedRoute, private store: Store) { }

	colors = Color;
	private returnUrl = '';
	private destroyed$ = new Subject();
	externalProviders$ = new BehaviorSubject<AuthenticationScheme[]>([]);
	errorMessages$ = new BehaviorSubject<ErrorMessage[]>([]);

	ngOnInit() {
		this.route.queryParams.pipe(takeUntil(this.destroyed$)).subscribe((params) => {
			this.returnUrl = params['return'] || '/';
		});
		this.accountService.getExternalLoginProviders().subscribe({
			next: (providers) => {
				this.externalProviders$.next(providers);
			}, error: (error) => {
				this.errorMessages$.pipe(tap((errorMessages) => {
					errorMessages.push({ message: 'Cannot get external providers', color: this.colors.danger });
				})).subscribe();
			}
		});
	}

	ngOnDestroy() {
		this.destroyed$.next(true);
	}

	externalLoginSuccessOrFailed(result: ExternalLoginResult) {
		console.log('externalLoginSuccessOrFailed', result);
		switch (result.status) {
			case ELoginStatus.success: {
				// We can't rely on the user we receive from the popup.
				this.accountService.currentUser().subscribe({
					next: (user) => {
						this.store.dispatch([
							new SetUser(user)
						]);
						this.router.navigateByUrl(this.returnUrl);
					}, error: (error) => {
						this.errorMessages$.pipe(tap((errorMessages) => {
							errorMessages.push({ message: 'Unable to fetch the current user', color: this.colors.danger });
						})).subscribe();
					}
				})
			} break;
			default: {
				this.errorMessages$.pipe(tap((errorMessages) => {
					errorMessages.push({ message: result.errorDescription, color: this.colors.danger });
				})).subscribe();
			}
		}
	}

	removeErrorMessage(message: ErrorMessage, isVisible: boolean) {
		if (!isVisible) {
			this.errorMessages$.pipe(tap((errorMessages) => {
				errorMessages.splice(errorMessages.indexOf(message), 1);
			})).subscribe();
		}
	}
}
