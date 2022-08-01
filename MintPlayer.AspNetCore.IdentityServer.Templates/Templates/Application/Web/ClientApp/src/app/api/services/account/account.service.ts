import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrlService } from '@mintplayer/ng-base-url';
import { AuthenticationScheme } from '../../dtos/authentication-scheme';
import { User } from '../../dtos/user';

@Injectable({
	providedIn: 'root'
})
export class AccountService {

	constructor(private httpClient: HttpClient, baseUrlService: BaseUrlService) {
		this.baseUrl = baseUrlService.getBaseUrl({ dropScheme: true });
	}

	private baseUrl: string | null;

	public csrfRefresh() {
		return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/Csrf-Refresh`, {});
	}

	public currentUser() {
		return this.httpClient.get<User>(`${this.baseUrl}/Web/V1/Account/CurrentUser`);
	}

	public getExternalLoginProviders() {
		return this.httpClient.get<AuthenticationScheme[]>(`${this.baseUrl}/Web/V1/Account/ExternalLogin/Providers`);
	}

	public logout() {
		return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/Logout`, {});
	}

}
