import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Select, Store } from '@ngxs/store';
import { BehaviorSubject, combineLatest, filter, map, Observable, Subject, take, tap } from 'rxjs';
import { ApplicationManager } from '../../../states/application/application.manager';
import { SetTwoFactor } from '../../../states/application/actions/set-two-factor';
import { SetBypassTwoFactor } from '../../../states/application/actions/set-two-factor-bypass';
import { ECodeType } from '../../../api/enums/code-type';
import { TwoFactorCode } from '../../../api/dtos/two-factor-code';
import { AccountService } from '../../../api/services/account/account.service';
import { User } from '../../../api/dtos/user';
import { TwoFactorCodeUI } from '../../../entities/two-factor-code-ui';
import { TwoFactorCodeModal } from '../../../entities/two-factor-code-modal';
import { ChangePasswordModal } from '../../../entities/change-password-modal';
import { AuthenticationScheme } from '../../../api/dtos/authentication-scheme';
import { ExternalLoginProviderInfo } from '../../../api/dtos/external-login-provider-info';
import { ExternalLoginResult } from '../../../api/dtos/external-login-result';
import { ELoginStatus } from '../../../api/enums/login-status';

@Component({
	selector: 'app-profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

	constructor(private accountService: AccountService, private domSanitizer: DomSanitizer, private store: Store) {
		this.twoFaRegistrationUrlSanitized$ = this.twoFaRegistrationUrl$
			.pipe(map((url) => {
				if (url) {
					return this.domSanitizer.bypassSecurityTrustUrl(url);
				} else {
					return null;
				}
			}));

		this.externalLoginProviders$ = combineLatest([this.allExternalLoginProviders$, this.registeredExternalLoginProviders$])
			.pipe(map(([allExternalLoginProviders, registeredExternalLoginProviders]) => {
				if (allExternalLoginProviders && registeredExternalLoginProviders) {
					return allExternalLoginProviders.map(p => {
						return <ExternalLoginProviderInfo>{
							name: p.name,
							displayName: p.displayName,
							isRegistered: registeredExternalLoginProviders.includes(p.name),
						};
					})
				} else {
					return null;
				}
			}));
	}

	@Select(ApplicationManager.user) user$!: Observable<User>;

	twoFaRegistrationUrl$ = new BehaviorSubject<string | null>(null);
	twoFaRegistrationUrlSanitized$: Observable<SafeUrl | null>;
	enteredTwoFaCode$ = new Subject<TwoFactorCodeUI | null>();
	twoFactorCodeProcessed$ = new Subject<boolean>();
	recoveryCodes$ = new BehaviorSubject<string[] | null>(null);
	numberOfRecoveryCodesLeft$ = new BehaviorSubject<number | null>(null);
	allExternalLoginProviders$ = new BehaviorSubject<AuthenticationScheme[] | null>(null);
	registeredExternalLoginProviders$ = new BehaviorSubject<string[] | null>(null);
	externalLoginProviders$: Observable<ExternalLoginProviderInfo[] | null>;


	changePasswordModal$ = new BehaviorSubject<ChangePasswordModal | null>({
		isChangingPassword: false,
		currentPassword: null,
		newPassword: '',
		newPasswordConfirmation: '',
	});
	twoFactorCodeModal$ = new BehaviorSubject<TwoFactorCodeModal | null>({
		isRequestingTwoFactorCode: false,
		twoFactorCode: {
			verificationCode: '',
			recoveryCode: '',
			type: ECodeType.verificationCode,
		},
		allowRecoveryCode: false,
	});

	ngOnInit() {
		this.accountService.hasPassword().subscribe({
			next: (hasPassword) => {
				this.changePasswordModal$.pipe(tap((m) => {
					if (m) {
						m.currentPassword = hasPassword ? '' : null;
					}
				}), take(1)).subscribe((m) => {
					this.changePasswordModal$.next(m);
				});
			},
			error: (error) => {
				console.error(error);
			}
		});

		this.accountService.getTwoFactorRegistrationInfo().subscribe({
			next: (info) => {
				this.twoFaRegistrationUrl$.next(info.registrationUrl);
			}, error: (error) => {
				console.error(error);
			}
		});

		this.accountService.getRemainingRecoveryCodes().subscribe({
			next: (count) => {
				this.numberOfRecoveryCodesLeft$.next(count);
			}, error: (error) => {
				console.error(error);
			}
		});

		this.accountService.getExternalLoginProviders().subscribe({
			next: (providers) => {
				this.allExternalLoginProviders$.next(providers);
			}, error: (error) => {
				console.error(error);
			}
		});

		this.accountService.getRegisteredExternalLoginProviders().subscribe({
			next: (providers) => {
				this.registeredExternalLoginProviders$.next(providers);
			}, error: (error) => {
				console.error(error);
			}
		});
	}

	convertTwoFactorCode(code: TwoFactorCodeUI) {
		switch (code.type) {
			case ECodeType.verificationCode:
				return <TwoFactorCode>{
					type: code.type,
					code: code.verificationCode,
				};
			case ECodeType.recoveryCode:
				return <TwoFactorCode>{
					type: code.type,
					code: code.recoveryCode,
				};
			default:
				throw 'Wrong code type';
		}
	}

	changePassword(show: boolean) {
		this.changePasswordModal$.pipe(take(1)).subscribe((modal) => {
			if (modal) {
				this.changePasswordModal$.next({
					...modal,
					isChangingPassword: true,
				});
			}
		});
	}

	updatePassword() {
		this.changePasswordModal$.pipe(take(1)).subscribe((modal) => {
			if (modal) {
				this.accountService.changePassword(modal.currentPassword, modal.newPassword, modal.newPasswordConfirmation).subscribe({
					next: () => {
						this.changePasswordModal$.next({
							isChangingPassword: false,
							currentPassword: '',
							newPassword: '',
							newPasswordConfirmation: '',
						});
					},
					error: (error) => { }
				});
			}
		});
	}

	setEnableTwoFactor(enable: boolean) {
		this.enteredTwoFaCode$.pipe(take(1), filter((code) => code !== null)).subscribe((code) => {
			this.accountService.setTwoFactorEnabled(enable, this.convertTwoFactorCode(code!)).subscribe({
				next: () => {
					this.twoFactorCodeModal$.next({
						isRequestingTwoFactorCode: false,
						twoFactorCode: {
							verificationCode: '',
							recoveryCode: '',
							type: ECodeType.verificationCode,
						},
						allowRecoveryCode: false,
					});
					this.store.dispatch([
						new SetTwoFactor(enable)
					]);
				}, error: (error) => {
				}
			});
		});
		this.twoFactorCodeModal$.next({
			isRequestingTwoFactorCode: true,
			twoFactorCode: {
				verificationCode: '',
				recoveryCode: '',
				type: ECodeType.verificationCode,
			},
			allowRecoveryCode: !enable,
		});
	}

	twoFactorCodeEntered() {
		this.enteredTwoFaCode$.next(this.twoFactorCodeModal$.value?.twoFactorCode ?? null);
	}

	dismissTwoFactorModal() {
		this.enteredTwoFaCode$.next(null);
	}

	setBypass2faForExternalLogin(bypass: boolean) {
		this.enteredTwoFaCode$.pipe(take(1), filter((code) => code !== null)).subscribe((code) => {
			this.accountService.setBypassTwoFactorForExternallogin(bypass, code?.verificationCode!).subscribe({
				next: () => {
					this.twoFactorCodeModal$.next({
						isRequestingTwoFactorCode: false,
						twoFactorCode: {
							verificationCode: '',
							recoveryCode: '',
							type: ECodeType.verificationCode,
						},
						allowRecoveryCode: false,
					});
					this.store.dispatch([
						new SetBypassTwoFactor(bypass)
					]);
				}, error: (error) => {
				}
			});
		});
		this.twoFactorCodeModal$.next({
			isRequestingTwoFactorCode: true,
			twoFactorCode: {
				verificationCode: '',
				recoveryCode: '',
				type: ECodeType.verificationCode,
			},
			allowRecoveryCode: false,
		});
	}

	generateNewRecoveryCodes() {
		this.enteredTwoFaCode$.pipe(take(1), filter((code) => code !== null)).subscribe((code) => {
			this.accountService.generateNewRecoveryCodes(code?.verificationCode!).subscribe({
				next: (codes) => {
					this.twoFactorCodeModal$.next({
						isRequestingTwoFactorCode: false,
						twoFactorCode: {
							verificationCode: '',
							recoveryCode: '',
							type: ECodeType.verificationCode,
						},
						allowRecoveryCode: false,
					});
					this.recoveryCodes$.next(codes);
				}, error: (error) => {
				}
			});
		});
		this.twoFactorCodeModal$.next({
			isRequestingTwoFactorCode: true,
			twoFactorCode: {
				verificationCode: '',
				recoveryCode: '',
				type: ECodeType.verificationCode,
			},
			allowRecoveryCode: false,
		});
	}

	setFocus(value: boolean, element: HTMLElement) {
		if (value) {
			setTimeout(() => element.focus(), 10);
		}
	}

	externalLoginSuccessOrFailed(result: ExternalLoginResult) {
		if (result.status === ELoginStatus.success) {
			this.registeredExternalLoginProviders$.pipe(take(1), map((providers) => {
				providers?.push(result.provider);
				return providers;
			})).subscribe({
				next: (providers) => {
					if (providers) {
						this.registeredExternalLoginProviders$.next(providers);
					} else {
						this.registeredExternalLoginProviders$.next([]);
					}
				}, error: (error) => {
					console.error(error);
				}
			});
		}
	}

	removeExternalLogin(provider: string) {
		this.accountService.removeExternalLoginProvider(provider).subscribe({
			next: () => {
				this.registeredExternalLoginProviders$.pipe(take(1), map((providers) => {
					if (providers) {
						providers?.splice(providers.indexOf(provider), 1);
						return providers;
					} else {
						return null;
					}
				})).subscribe((providers) => {
					this.registeredExternalLoginProviders$.next(providers);
				});
			}, error: (error) => {
				console.error(error);
			}
		});
	}

}
