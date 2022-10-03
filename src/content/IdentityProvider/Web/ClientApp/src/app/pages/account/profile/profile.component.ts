import { Component, OnDestroy, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Select, Store } from '@ngxs/store';
import { BehaviorSubject, combineLatest, filter, map, Observable, Subject, take, takeUntil, tap } from 'rxjs';
import { ApplicationManager } from '../../../states/application/application.manager';
import { ECodeType } from '../../../api/enums/code-type';
import { AccountService } from '../../../api/services/account/account.service';
import { User } from '../../../api/dtos/user';
//#if (UseTwoFactorAuthentication)
import { SetTwoFactor } from '../../../states/application/actions/set-two-factor';
import { SetBypassTwoFactor } from '../../../states/application/actions/set-two-factor-bypass';
import { TwoFactorCode } from '../../../api/dtos/two-factor-code';
import { TwoFactorCodeUI } from '../../../entities/two-factor-code-ui';
import { TwoFactorCodeModal } from '../../../entities/two-factor-code-modal';
//#endif
import { ChangePasswordModal } from '../../../entities/change-password-modal';
//#if (UseExternalLogins)
import { AuthenticationScheme } from '../../../api/dtos/authentication-scheme';
import { ExternalLoginProviderInfo } from '../../../api/dtos/external-login-provider-info';
import { ExternalLoginResult } from '../../../api/dtos/external-login-result';
//#endif
import { ELoginStatus } from '../../../api/enums/login-status';
import { SetupService } from '../../../api/services/setup/setup.service';
import { DeveloperPortalAppInformation } from '../../../api/dtos/developer-portal-app-information';

@Component({
	selector: 'app-profile',
	templateUrl: './profile.component.html',
	styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy {

	constructor(private accountService: AccountService, private setupService: SetupService, private domSanitizer: DomSanitizer, private store: Store) {
//#if (UseTwoFactorAuthentication)
		this.twoFaRegistrationUrlSanitized$ = this.twoFaRegistrationUrl$
			.pipe(map((url) => {
				if (url) {
					return this.domSanitizer.bypassSecurityTrustUrl(url);
				} else {
					return null;
				}
			}));

//#endif
//#if (UseExternalLogins)
		this.externalLoginProviders$ = combineLatest([this.allExternalLoginProviders$, this.registeredExternalLoginProviders$])
			.pipe(map(([allExternalLoginProviders, registeredExternalLoginProviders]) => {
				if (!allExternalLoginProviders || !registeredExternalLoginProviders) {
					return null;
				} else if (allExternalLoginProviders.length === 0) {
					return null;
				} else {
					return allExternalLoginProviders.map(p => {
						return <ExternalLoginProviderInfo>{
							name: p.name,
							displayName: p.displayName,
							isRegistered: registeredExternalLoginProviders.includes(p.name),
						};
					});
				}
			}));

//#endif
		this.userRoles$
			.pipe(takeUntil(this.destroyed$))
			.subscribe((roles) => {
				if (roles && roles.includes('Administrator')) {
					this.setupService.isDeveloperPortalClientRegistered()
						.subscribe((isRegistered) => {
							this.developerPortalAppInformation$.next({
								isRegistered
							});
						});
				}
			});
	}

	@Select(ApplicationManager.user) user$!: Observable<User>;

//#if (UseTwoFactorAuthentication)
	twoFaRegistrationUrl$ = new BehaviorSubject<string | null>(null);
	twoFaRegistrationUrlSanitized$: Observable<SafeUrl | null>;
	enteredTwoFaCode$ = new Subject<TwoFactorCodeUI | null>();
	twoFactorCodeProcessed$ = new Subject<boolean>();
	recoveryCodes$ = new BehaviorSubject<string[] | null>(null);
	numberOfRecoveryCodesLeft$ = new BehaviorSubject<number | null>(null);
//#endif
//#if (UseExternalLogins)
	allExternalLoginProviders$ = new BehaviorSubject<AuthenticationScheme[] | null>(null);
	registeredExternalLoginProviders$ = new BehaviorSubject<string[] | null>(null);
	externalLoginProviders$: Observable<ExternalLoginProviderInfo[] | null>;
//#endif
	userRoles$ = new BehaviorSubject<string[] | null>(null);
	developerPortalAppInformation$ = new BehaviorSubject<DeveloperPortalAppInformation | null>(null);
	developerPortalRedirectUrl = '';
	destroyed$ = new Subject();


	changePasswordModal$ = new BehaviorSubject<ChangePasswordModal | null>({
		isChangingPassword: false,
		currentPassword: null,
		newPassword: '',
		newPasswordConfirmation: '',
	});

//#if (UseTwoFactorAuthentication)
	twoFactorCodeModal$ = new BehaviorSubject<TwoFactorCodeModal | null>({
		isRequestingTwoFactorCode: false,
		twoFactorCode: {
			verificationCode: '',
			recoveryCode: '',
			type: ECodeType.verificationCode,
		},
		allowRecoveryCode: false,
	});

//#endif
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

//#if (UseTwoFactorAuthentication)
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

//#endif
//#if (UseExternalLogins)
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

//#endif
		this.accountService.getRoles().subscribe({
			next: (roles) => this.userRoles$.next(roles),
			error: (error) => console.error(error)
		});
	}

	ngOnDestroy() {
		this.destroyed$.next(true);
	}

//#if (UseTwoFactorAuthentication)
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

//#endif
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

//#if (UseTwoFactorAuthentication)
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

//#endif
	setFocus(value: boolean, element: HTMLElement) {
		if (value) {
			setTimeout(() => element.focus(), 10);
		}
	}

//#if (UseExternalLogins)
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

//#endif
	createDeveloperPortal() {
		this.setupService.registerDeveloperPortalClient({ redirectUris: [this.developerPortalRedirectUrl] })
			.subscribe({
				next: (response) => {
					this.developerPortalAppInformation$.next({
						isRegistered: true,
						clientId: response.clientId,
						clientSecret: response.clientSecret,
					});
				},
				error: (error) => console.error(error)
			});
	}

}
