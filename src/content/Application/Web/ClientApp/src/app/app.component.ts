import { isPlatformServer } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { concatMap, Observable } from 'rxjs';
import { User } from './api/dtos/user';
import { AccountService } from './api/services/account/account.service';
import { DataFromServer } from './interfaces/data-from-server';
import { DATA_FROM_SERVER } from './providers/data-from-server';
import { Logout } from './states/application/actions/logout';
import { SetUser } from './states/application/actions/set-user';
import { ApplicationManager } from './states/application/application.manager';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

	constructor(
		@Inject(PLATFORM_ID) private platformId: Object,
		private accountService: AccountService,
		private store: Store,
		@Inject(DATA_FROM_SERVER) private dataFromServer: DataFromServer
	) {
	}

	title = 'ClientApp';

	@Select(ApplicationManager.user) user$!: Observable<User>;

	ngOnInit() {
		if (isPlatformServer(this.platformId)) {
			this.store.dispatch([
				new SetUser(this.dataFromServer.user)
			]);
		} else {
			this.accountService.currentUser().subscribe({
				next: (user) => {
					this.store.dispatch([
						new SetUser(user)
					]);
				},
				error: (error) => {
				}
			});
		}
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
