import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Color } from '@mintplayer/ng-bootstrap';
import { AdvancedRouter } from '@mintplayer/ng-router';
import { Store } from '@ngxs/store';
import { BehaviorSubject, concatMap, map, of, Subject, takeUntil, tap } from 'rxjs';
import { ErrorMessage } from '../../../entities/error-message';
import { ELoginStatus } from '../../../api/enums/login-status';
import { AccountService } from '../../../api/services/account/account.service';
import { SetUser } from '../../../states/application/actions/set-user';
import { AuthenticationScheme } from '../../../api/dtos/authentication-scheme';
import { ExternalLoginResult } from '../../../api/dtos/external-login-result';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnDestroy {

	constructor(private accountService: AccountService, private router: AdvancedRouter, private route: ActivatedRoute, private store: Store) { }

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

	login() {
		this.accountService.login(this.email, this.password).pipe(
			concatMap((loginResult) => {
				switch (loginResult.status) {
					case ELoginStatus.success:
						return this.accountService.csrfRefresh().pipe(map(() => loginResult));
					case ELoginStatus.notActivated:
					case ELoginStatus.requiresTwoFactor:
						return of(loginResult);
					default:
						throw new Error('Something went wrong');
				}
			})
		).subscribe({
			next: (loginResult) => {
				switch (loginResult.status) {
					case ELoginStatus.success:
						this.store.dispatch([
							new SetUser(loginResult.user)
						]);
						this.router.navigateByUrl(this.returnUrl);
						break;
					case ELoginStatus.notActivated:
						this.errorMessages$.pipe(tap((errorMessages) => {
							errorMessages.push({ message: 'Your account isn\'t confirmed yet', color: this.colors.warning });
						})).subscribe();
						break;
					case ELoginStatus.requiresTwoFactor:
						this.router.navigate(
							['/account', 'two-factor'],
							{
								queryParams: { return: this.returnUrl }
							}
						);
						break;
				}
			},
			error: (error) => {
				this.errorMessages$.pipe(tap((errorMessages) => {
					errorMessages.push({ message: 'Login unsuccessful', color: this.colors.danger });
				})).subscribe();
			}
		});
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

	email = '';
	password = '';
	colors = Color;
	private returnUrl = '';
	private destroyed$ = new Subject();

	externalProviders$ = new BehaviorSubject<AuthenticationScheme[]>([]);
	errorMessages$ = new BehaviorSubject<ErrorMessage[]>([]);
	removeErrorMessage(message: ErrorMessage, isVisible: boolean) {
		if (!isVisible) {
			this.errorMessages$.pipe(tap((errorMessages) => {
				errorMessages.splice(errorMessages.indexOf(message), 1);
			})).subscribe();
		}
	}

}
