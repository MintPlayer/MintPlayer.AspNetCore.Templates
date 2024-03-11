import { isPlatformServer } from '@angular/common';
import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Color } from '@mintplayer/ng-bootstrap';
import { BsNavbarModule } from '@mintplayer/ng-bootstrap/navbar';
import { Select, Store } from '@ngxs/store';
import { concatMap, Observable } from 'rxjs';
import { User } from './api/dtos/user';
import { AccountService } from './api/services/account/account.service';
import { DataFromServer } from './interfaces/data-from-server';
////#if (SsrOnSupplyData)
import { DATA_FROM_SERVER } from './providers/data-from-server';
////#endif
import { Logout } from './states/application/actions/logout';
import { SetUser } from './states/application/actions/set-user';
import { ApplicationManager } from './states/application/application.manager';

@Component({
	selector: 'app-root',
	standalone: true,
	imports: [
		RouterOutlet,
		BsNavbarModule
	],
	templateUrl: './app.component.html',
	styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

	constructor(
		@Inject(PLATFORM_ID) private platformId: Object,
		private accountService: AccountService,
		private store: Store,
////#if (SsrOnSupplyData)
		@Inject(DATA_FROM_SERVER) private dataFromServer: DataFromServer,
////#endif
	) {
	}

	title = 'ClientApp';
	colors = Color;

	@Select(ApplicationManager.user) user$!: Observable<User>;

	ngOnInit() {
////#if (SsrOnSupplyData)
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
////#else
		this.accountService.currentUser().subscribe({
			next: (user) => {
				this.store.dispatch([
					new SetUser(user)
				]);
			},
			error: (error) => {
			}
		});
////#endif
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
