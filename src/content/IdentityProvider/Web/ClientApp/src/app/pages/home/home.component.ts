import { Component, OnInit } from '@angular/core';
import { Color } from '@mintplayer/ng-bootstrap';
import { BehaviorSubject, concatMap, map, Observable, of } from 'rxjs';
import { AccountService } from '../../api/services/account/account.service';
import { SetupService } from '../../api/services/setup/setup.service';

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

	constructor(private accountService: AccountService, private setupService: SetupService) {
		this.mustCreateDeveloperPortalApp$ = this.userRoles$.pipe(
			concatMap((roles) => {
				if (!roles) {
					return of(false);
				} else if (roles.includes('Administrator')) {
					return this.setupService.isDeveloperPortalClientRegistered().pipe(map((isRegistered) => !isRegistered));
				} else {
					return of(false);
				}
			}));
	}

	colors = Color;

	userRoles$ = new BehaviorSubject<string[] | null>(null);
	mustCreateDeveloperPortalApp$: Observable<boolean>;

	ngOnInit() {
		this.accountService.getRoles().subscribe({
			next: (roles) => this.userRoles$.next(roles),
			error: (error) => console.error(error)
		});
	}
}
