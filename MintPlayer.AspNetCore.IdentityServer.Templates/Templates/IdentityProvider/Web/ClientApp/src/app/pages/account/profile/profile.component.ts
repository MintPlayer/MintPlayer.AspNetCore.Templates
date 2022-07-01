import { Component, OnInit } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { BehaviorSubject, filter, map, Observable, Subject, take, tap } from 'rxjs';
import { AccountService } from '../../../services/account/account.service';
import { User } from '../../../entities/user';
import { ApplicationManager } from '../../../states/application/application.manager';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { SetTwoFactor } from 'src/app/states/application/actions/set-two-factor';
import { SetBypassTwoFactor } from '../../../states/application/actions/set-two-factor-bypass';

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

    isChangingPassword = false;
    currentPassword: string | null = null;
    newPassword = '';
    newPasswordConfirmation = '';
    hasPassword$ = new BehaviorSubject<boolean | null>(null);
    isRequestingTwoFactorCode = false;
    verificationCode = '';
    twoFaRegistrationUrl$ = new BehaviorSubject<string | null>(null);
    twoFaRegistrationUrlSanitized$: Observable<SafeUrl | null>;
    enteredTwoFaCode$ = new Subject<string | null>();
    twoFactorCodeProcessed$ = new Subject<boolean>();
    backupCodes$ = new BehaviorSubject<string[] | null>(null);
    numberOfRecoveryCodesLeft$ = new BehaviorSubject<number | null>(null);

    ngOnInit() {
        this.accountService.hasPassword().subscribe({
            next: (hasPassword) => {
                this.hasPassword$.next(hasPassword);
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

    updatePassword() {
        this.accountService.changePassword(this.currentPassword, this.newPassword, this.newPasswordConfirmation).subscribe({
            next: () => {
                this.currentPassword = this.newPassword = this.newPasswordConfirmation = '';
                this.isChangingPassword = false;
            },
            error: (error) => { }
        });
    }

    setEnableTwoFactor(enable: boolean) {
        this.enteredTwoFaCode$.pipe(take(1), filter((code) => code !== null)).subscribe((code) => {
            this.accountService.setTwoFactorEnabled(enable, code!).subscribe({
                next: () => {
                    this.isRequestingTwoFactorCode = false;
                    this.verificationCode = '';
                    this.store.dispatch([
                        new SetTwoFactor(enable)
                    ]);
                }, error: (error) => {
                }
            });
        });
        this.isRequestingTwoFactorCode = true;
    }

    twoFactorCodeEntered() {
        this.enteredTwoFaCode$.next(this.verificationCode);
    }

    dismissTwoFactorModal() {
        this.enteredTwoFaCode$.next(null);
    }

    setBypass2faForExternalLogin(bypass: boolean) {
        this.enteredTwoFaCode$.pipe(take(1), filter((code) => code !== null)).subscribe((code) => {
            this.accountService.setBypassTwoFactorForExternallogin(bypass, code!).subscribe({
                next: () => {
                    this.isRequestingTwoFactorCode = false;
                    this.verificationCode = '';
                    this.store.dispatch([
                        new SetBypassTwoFactor(bypass)
                    ]);
                }, error: (error) => {
                }
            });
        });
        this.isRequestingTwoFactorCode = true;
    }

    generateNewRecoveryCodes() {
        this.enteredTwoFaCode$.pipe(take(1), filter((code) => code !== null)).subscribe((code) => {
            this.accountService.generateNewRecoveryCodes(code!).subscribe({
                next: (codes) => {
                    this.isRequestingTwoFactorCode = false;
                    this.verificationCode = '';
                    this.backupCodes$.next(codes);
                }, error: (error) => {
                }
            });
        });
        this.isRequestingTwoFactorCode = true;
    }

}
