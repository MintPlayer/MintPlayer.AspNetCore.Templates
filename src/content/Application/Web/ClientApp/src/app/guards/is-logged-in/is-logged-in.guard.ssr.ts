import { isPlatformServer } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AdvancedRouter } from '@mintplayer/ng-router';
import { AccountService } from '../../api/services/account/account.service';
import { DATA_FROM_SERVER } from '../../providers/data-from-server';

@Injectable({
  providedIn: 'root'
})
export class IsLoggedInGuard implements CanActivate {
	constructor(
		private accountService: AccountService,
		private router: AdvancedRouter,
		@Inject(PLATFORM_ID) private platformId: Object,
		@Inject(DATA_FROM_SERVER) private dataFromServer: any
	) { }

	canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
		if (isPlatformServer(this.platformId)) {
			if (this.dataFromServer.user) {
				return true;
			} else {
				return false;
			}
		} else {
			return this.accountService.currentUser().toPromise().then((user) => {
				return true;
			}).catch((error: HttpErrorResponse) => {
				this.router.navigate(['/account', 'login'], {
					queryParams: {
						return: state.url
					}
				});
				return false;
			});
		}
	}
}
