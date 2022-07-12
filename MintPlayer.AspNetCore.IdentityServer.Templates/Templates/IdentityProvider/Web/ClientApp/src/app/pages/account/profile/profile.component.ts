import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Select, Store } from '@ngxs/store';
import { BehaviorSubject, filter, map, Observable, Subject, take, tap } from 'rxjs';
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
	}

	@Select(ApplicationManager.user) user$!: Observable<User>;

	twoFaRegistrationUrl$ = new BehaviorSubject<string | null>(null);
	twoFaRegistrationUrlSanitized$: Observable<SafeUrl | null>;
	enteredTwoFaCode$ = new Subject<TwoFactorCodeUI | null>();
	twoFactorCodeProcessed$ = new Subject<boolean>();
	recoveryCodes$ = new BehaviorSubject<string[] | null>(null);
	numberOfRecoveryCodesLeft$ = new BehaviorSubject<number | null>(null);


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

}
