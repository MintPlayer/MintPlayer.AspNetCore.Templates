import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseUrlService } from '@mintplayer/ng-base-url';
import { User } from '../../dtos/user';
import { LoginResult } from '../../dtos/login-result';
import { RegisterResult } from '../../dtos/register-result';
//#if (UseTwoFactorAuthentication)
import { TwoFactorRegistrationInfo } from '../../dtos/two-factor-registration-info';
import { TwoFactorCode } from '../../dtos/two-factor-code';
//#endif
//#if (UseExternalLogins)
import { AuthenticationScheme } from '../../dtos/authentication-scheme';
//#endif

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

	public register(user: User, password: string, passwordConfirmation: string) {
		return this.httpClient.post<RegisterResult>(`${this.baseUrl}/Web/V1/Account/Register`, { user, password, passwordConfirmation });
	}

	public login(email: string, password: string) {
		return this.httpClient.post<LoginResult>(`${this.baseUrl}/Web/V1/Account/Login`, { email, password });
	}

//#if (UseTwoFactorAuthentication)
	public twoFactorLogin(verificationCode: string, rememberDevice: boolean) {
		return this.httpClient.post<User>(`${this.baseUrl}/Web/V1/Account/TwoFactor/Login`, { verificationCode, rememberDevice });
	}

//#endif
//#if (UseEmailConfirmation)
	public resendConfirmationEmail(email: string) {
		return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/Resend`, { email });
	}

//#endif
	public currentUser() {
		return this.httpClient.get<User>(`${this.baseUrl}/Web/V1/Account/CurrentUser`);
	}

	public hasPassword() {
		return this.httpClient.get<boolean>(`${this.baseUrl}/Web/V1/Account/Password`);
	}

	public changePassword(currentPassword: string | null, newPassword: string, newPasswordConfirmation: string) {
		return this.httpClient.put(`${this.baseUrl}/Web/V1/Account/Password`, { currentPassword, newPassword, newPasswordConfirmation });
	}

	public performMustChangePassword(newPassword: string, newPasswordConfirmation: string) {
		return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/Password`, { newPassword, newPasswordConfirmation });
	}

	public getRoles() {
		return this.httpClient.get<string[]>(`${this.baseUrl}/Web/V1/Account/Roles`);
	}

	public logout() {
		return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/Logout`, {});
	}

//#if (UseTwoFactorAuthentication)
	public getTwoFactorRegistrationInfo() {
		return this.httpClient.post<TwoFactorRegistrationInfo>(`${this.baseUrl}/Web/V1/Account/TwoFactor/RegistrationInfo`, {});
	}

	public setTwoFactorEnabled(enable: boolean, code: TwoFactorCode) {
		return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/TwoFactor`, { enable, code });
	}

	public getRemainingRecoveryCodes() {
		return this.httpClient.get<number>(`${this.baseUrl}/Web/V1/Account/TwoFactor/Recovery/RemainingCodes`);
	}

	public generateNewRecoveryCodes(verificationCode: string) {
		return this.httpClient.put<string[]>(`${this.baseUrl}/Web/V1/Account/TwoFactor/Recovery/RemainingCodes`, { verificationCode });
	}

	public setBypassTwoFactorForExternallogin(bypass: boolean, verificationCode: string) {
		return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/TwoFactor/Bypass`, { bypass, verificationCode });
	}

	public twoFactorRecovery(recoveryCode: string) {
		return this.httpClient.post<User>(`${this.baseUrl}/Web/V1/Account/TwoFactor/Recovery`, { recoveryCode });
	}

//#endif
//#if (UseExternalLogins)
	public getExternalLoginProviders() {
		return this.httpClient.get<AuthenticationScheme[]>(`${this.baseUrl}/Web/V1/Account/ExternalLogin/Providers`);
	}

	public getRegisteredExternalLoginProviders() {
		return this.httpClient.get<string[]>(`${this.baseUrl}/Web/V1/Account/ExternalLogin`);
	}

	public removeExternalLoginProvider(provider: string) {
		return this.httpClient.delete(`${this.baseUrl}/Web/V1/Account/ExternalLogin/${provider}`);
	}

//#endif
}
