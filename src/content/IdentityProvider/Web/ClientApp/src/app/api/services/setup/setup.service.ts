import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseUrlService } from '@mintplayer/ng-base-url';
import { CreateDeveloperPortalRequest } from '../../dtos/create-developer-portal-request';
import { CreateDeveloperPortalResponse } from '../../dtos/create-developer-portal-response';

@Injectable({
	providedIn: 'root'
})
export class SetupService {

	constructor(private httpClient: HttpClient, baseUrlService: BaseUrlService) {
		this.baseUrl = baseUrlService.getBaseUrl({ dropScheme: true });
	}

	private baseUrl: string | null;

	public isDeveloperPortalClientRegistered() {
		return this.httpClient.get<boolean>(`${this.baseUrl}/Web/V1/Setup`);
	}

	public registerDeveloperPortalClient(request: CreateDeveloperPortalRequest) {
		return this.httpClient.post<CreateDeveloperPortalResponse>(`${this.baseUrl}/Web/V1/Setup`, request);
	}
}
