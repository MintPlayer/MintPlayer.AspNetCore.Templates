import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Color } from '@mintplayer/ng-bootstrap';
import { AdvancedRouter } from '@mintplayer/ng-router';
import { Store } from '@ngxs/store';
import { Guid } from 'guid-typescript';
import { BehaviorSubject, concatMap, map, Subject, takeUntil } from 'rxjs';
import { SetUser } from '../../../states/application/actions/set-user';
import { ELoginStatus } from '../../../api/enums/login-status';
import { AccountService } from '../../../api/services/account/account.service';
import { User } from '../../../api/dtos/user';

@Component({
	selector: 'app-register',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {

	constructor(private accountService: AccountService, private router: AdvancedRouter, private route: ActivatedRoute, private store: Store) { }

	ngOnInit() {
		this.route.queryParams.pipe(takeUntil(this.destroyed$)).subscribe((params) => {
			this.returnUrl = params['return'] || '/';
		});
	}

	ngOnDestroy() {
		this.destroyed$.next(true);
	}

	register() {
		this.accountService.register(this.user, this.password, this.passwordConfirmation).pipe(
			concatMap((registerResult) => {
				if (registerResult.requiresEmailConfirmation) {
					throw new Error('An email has been sent with a link to confirm your account');
				} else {
					return this.accountService.login(this.user.email, this.password);
				}
			}),
			concatMap((loginResult) => {
				if (loginResult.status === ELoginStatus.success) {
					return this.accountService.csrfRefresh().pipe(map(() => loginResult));
				} else {
					throw new Error('Something went wrong');
				}
			})
		).subscribe({
			next: (loginResult) => {
				this.store.dispatch([
					new SetUser(loginResult.user)
				]);
				this.router.navigateByUrl(this.returnUrl);
			},
			error: (error) => {
				this.errorMessage$.next('Login unsuccessful');
			}
		});
	}

	user: User = { id: Guid.createEmpty()['value'], userName: '', email: '', isTwoFactorEnabled: false, bypass2faForExternalLogin: false };
	password = '';
	passwordConfirmation = '';
	colors = Color;
	private returnUrl = '';

	errorMessage$ = new BehaviorSubject<string | null>(null);
	private destroyed$ = new Subject();
}
