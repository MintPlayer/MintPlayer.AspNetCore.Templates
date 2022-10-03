import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Color } from '@mintplayer/ng-bootstrap';
import { AdvancedRouter } from '@mintplayer/ng-router';
import { Store } from '@ngxs/store';
import { BehaviorSubject, concatMap, map, of, Subject, take, takeUntil, tap } from 'rxjs';
import { ErrorMessage } from '../../../entities/error-message';
import { ELoginStatus } from '../../../api/enums/login-status';
import { AccountService } from '../../../api/services/account/account.service';
import { SetUser } from '../../../states/application/actions/set-user';
//#if (UseExternalLogins)
import { AuthenticationScheme } from '../../../api/dtos/authentication-scheme';
import { ExternalLoginResult } from '../../../api/dtos/external-login-result';
//#endif
import { ChangeAdminPasswordModal } from '../../../entities/change-admin-password-modal';
import { LoginResult } from '../../../api/dtos/login-result';

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
//#if (UseExternalLogins)
		this.accountService.getExternalLoginProviders().subscribe({
			next: (providers) => {
				this.externalProviders$.next(providers);
			}, error: (error) => {
				this.errorMessages$.pipe(tap((errorMessages) => {
					errorMessages.push({ message: 'Cannot get external providers', color: this.colors.danger });
				})).subscribe();
			}
		});
//#endif
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
					case ELoginStatus.mustChangePassword:
						return of(loginResult);
					default:
						throw new Error('Something went wrong');
				}
			})
		).subscribe({
			next: (loginResult) => this.processLoginResult(loginResult),
			error: (error) => {
				this.errorMessages$.pipe(tap((errorMessages) => {
					errorMessages.push({ message: 'Login unsuccessful', color: this.colors.danger });
				})).subscribe();
			}
		});
	}

	updateAdminPassword() {

		this.changePasswordModal$.pipe(take(1)).subscribe((changePasswordModal) => {
			this.accountService.performMustChangePassword(changePasswordModal.newPassword, changePasswordModal.newPasswordConfirmation).subscribe({
				next: () => {
					this.accountService.login(this.email, changePasswordModal.newPassword).pipe(
						concatMap((loginResult) => {
							switch (loginResult.status) {
								case ELoginStatus.success:
									return this.accountService.csrfRefresh().pipe(map(() => loginResult));
								case ELoginStatus.notActivated:
								case ELoginStatus.requiresTwoFactor:
								case ELoginStatus.mustChangePassword:
									return of(loginResult);
								default:
									throw new Error('Something went wrong');
							}
						})
					).subscribe({
						next: (loginResult) => {

							this.changePasswordModal$.pipe(tap((changePasswordModal) => {
								changePasswordModal.isChangingPassword = false;
								setTimeout(() => this.processLoginResult(loginResult), 20);
							})).subscribe();
						}, error: (error) => {
							this.errorMessages$.pipe(tap((errorMessages) => {
								errorMessages.push({ message: 'Login unsuccessful', color: this.colors.danger });
							}));
						}
					});
				},
				error: (error) => {
					this.errorMessages$.pipe(tap((errorMessages) => {
						errorMessages.push({ message: 'Login unsuccessful', color: this.colors.danger });
					}));
				}
			});
		});

	}

	processLoginResult(loginResult: LoginResult) {
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
//#if (UseExternalLogins)
			case ELoginStatus.requiresTwoFactor:
				this.router.navigate(
					['/account', 'two-factor'],
					{
						queryParams: { return: this.returnUrl }
					}
				);
				break;
//#endif
			case ELoginStatus.mustChangePassword:
				this.changePasswordModal$.pipe(tap((changePasswordModal) => {
					changePasswordModal.isChangingPassword = true;
					setTimeout(() => this.txtNewPassword.nativeElement.focus(), 20);
				})).subscribe();
				break;
		}
	}

//#if (UseExternalLogins)
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

//#endif
	email = '';
	password = '';
	colors = Color;
	private returnUrl = '';
	private destroyed$ = new Subject();

//#if (UseExternalLogins)
	externalProviders$ = new BehaviorSubject<AuthenticationScheme[]>([]);
//#endif
	errorMessages$ = new BehaviorSubject<ErrorMessage[]>([]);
	changePasswordModal$ = new BehaviorSubject<ChangeAdminPasswordModal>({
		isChangingPassword: false,
		bearerToken: null,
		newPassword: '',
		newPasswordConfirmation: '',
	});
	removeErrorMessage(message: ErrorMessage, isVisible: boolean) {
		if (!isVisible) {
			this.errorMessages$.pipe(tap((errorMessages) => {
				errorMessages.splice(errorMessages.indexOf(message), 1);
			})).subscribe();
		}
	}

	@ViewChild('txtNewPassword') txtNewPassword!: ElementRef<HTMLInputElement>;

}
