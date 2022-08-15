import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Color } from '@mintplayer/ng-bootstrap';
import { AdvancedRouter } from '@mintplayer/ng-router';
import { Store } from '@ngxs/store';
import { BehaviorSubject, concatMap, map, Subject, takeUntil, tap } from 'rxjs';
import { ErrorMessage } from '../../../entities/error-message';
import { AccountService } from '../../../api/services/account/account.service';
import { SetUser } from '../../../states/application/actions/set-user';

@Component({
	selector: 'app-two-factor',
	templateUrl: './two-factor.component.html',
	styleUrls: ['./two-factor.component.scss']
})
export class TwoFactorComponent implements OnInit {

	constructor(private accountService: AccountService, private router: AdvancedRouter, private route: ActivatedRoute, private store: Store) { }

	colors = Color;
	verificationCode = '';
	remember = false;
	isRequestingRecoveryCode = false;
	twoFactorRecoveryCode = '';
	errorMessages$ = new BehaviorSubject<ErrorMessage[]>([]);
	private returnUrl = '';
	private destroyed$ = new Subject();

	ngOnInit() {
		this.route.queryParams.pipe(takeUntil(this.destroyed$)).subscribe((params) => {
			this.returnUrl = params['return'] || '/';
		});
	}

	ngOnDestroy() {
		this.destroyed$.next(true);
	}

	verifyCode() {
		this.accountService.twoFactorLogin(this.verificationCode, this.remember).pipe(
			concatMap((user) => {
				return this.accountService.csrfRefresh().pipe(map(() => user));
			})
		).subscribe({
			next: (user) => {
				this.store.dispatch([
					new SetUser(user)
				]);
				this.router.navigateByUrl(this.returnUrl);
			}, error: (error) => {
				this.errorMessages$.pipe(tap((errorMessages) => {
					errorMessages.push({ message: 'The code is incorrect', color: this.colors.danger });
				})).subscribe();
			}
		});
	}

	removeErrorMessage(message: ErrorMessage, isVisible: boolean) {
		if (!isVisible) {
			this.errorMessages$.pipe(tap((errorMessages) => {
				errorMessages.splice(errorMessages.indexOf(message), 1);
			})).subscribe();
		}
	}

	OnLostDevice() {
		this.isRequestingRecoveryCode = true;
		return false;
	}

	recoveryCodeEntered() {
		this.accountService.twoFactorRecovery(this.twoFactorRecoveryCode).subscribe({
			next: (user) => {
				this.isRequestingRecoveryCode = false;
				setTimeout(() => {
					this.store.dispatch([
						new SetUser(user)
					]);
					this.router.navigateByUrl(this.returnUrl);
				}, 10);
			}, error: (error) => {
				this.errorMessages$.pipe(tap((errorMessages) => {
					errorMessages.push({ message: 'The code is incorrect', color: this.colors.danger });
				})).subscribe();
			}
		})
	}

}
