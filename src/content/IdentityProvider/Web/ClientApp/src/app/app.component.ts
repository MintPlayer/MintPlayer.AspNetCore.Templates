import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { concatMap, Observable } from 'rxjs';
import { Color } from '@mintplayer/ng-bootstrap';
import { User } from './api/dtos/user';
import { AccountService } from './api/services/account/account.service';
import { Logout } from './states/application/actions/logout';
import { SetUser } from './states/application/actions/set-user';
import { ApplicationManager } from './states/application/application.manager';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

	constructor(private accountService: AccountService, private store: Store) { }

	title = 'ClientApp';
	colors = Color;

	@Select(ApplicationManager.user) user$!: Observable<User>;

	ngOnInit() {
		this.accountService.currentUser().subscribe({
			next: (user) => {
				this.store.dispatch([
					new SetUser(user)
				]);
			},
			error: (error) => {
			}
		})
	}

	onLogout() {
		this.accountService.logout().pipe(
			concatMap(() => {
				return this.accountService.csrfRefresh();
			})
		).subscribe({
			next: () => {
				this.store.dispatch([
					new Logout()
				]);
			},
			error: (error) => {
			}
		});
	}

}
