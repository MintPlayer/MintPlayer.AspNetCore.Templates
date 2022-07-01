import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { AdvancedRouter } from '@mintplayer/ng-router';
import { catchError, map, of } from 'rxjs';
import { AccountService } from '../../services/account/account.service';

@Injectable({
    providedIn: 'root'
})
export class IsLoggedInGuard implements CanActivate {

    constructor(private accountService: AccountService, private router: AdvancedRouter) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.accountService.currentUser().pipe(
            catchError((error) => {
                this.router.navigate(['/account', 'login'], {
                    queryParams: {
                        return: state.url
                    }
                });
                return of(false);
            }),
            map((user) => {
                return true;
            })
        );
    }

}
