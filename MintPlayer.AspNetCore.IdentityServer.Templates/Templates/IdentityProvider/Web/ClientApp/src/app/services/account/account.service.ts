import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseUrlService } from '@mintplayer/ng-base-url';
import { User } from '../../entities/user';
import { LoginResult } from '../../entities/login-result';
import { TwoFactorRegistrationInfo } from '../../entities/two-factor-registration-info';
import { RegisterResult } from '../../entities/register-result';

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

    public twoFactorLogin(verificationCode: string, rememberDevice: boolean) {
        return this.httpClient.post<User>(`${this.baseUrl}/Web/V1/Account/TwoFactor/Login`, { verificationCode, rememberDevice });
    }

    public resendConfirmationEmail(email: string) {
        return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/Resend`, { email });
    }

    public currentUser() {
        return this.httpClient.get<User>(`${this.baseUrl}/Web/V1/Account/CurrentUser`);
    }

    public hasPassword() {
        return this.httpClient.get<boolean>(`${this.baseUrl}/Web/V1/Account/Password`);
    }

    public changePassword(currentPassword: string | null, newPassword: string, newPasswordConfirmation: string) {
        return this.httpClient.put(`${this.baseUrl}/Web/V1/Account/Password`, { currentPassword, newPassword, newPasswordConfirmation });
    }

    public getRoles() {
        return this.httpClient.get<string[]>(`${this.baseUrl}/Web/V1/Account/Roles`);
    }

    public logout() {
        return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/Logout`, {});
    }

    public getTwoFactorRegistrationInfo() {
        return this.httpClient.post<TwoFactorRegistrationInfo>(`${this.baseUrl}/Web/V1/Account/TwoFactor/RegistrationInfo`, {});
    }

    public setTwoFactorEnabled(enable: boolean, verificationCode: string) {
        return this.httpClient.post(`${this.baseUrl}/Web/V1/Account/TwoFactor`, { enable, verificationCode });
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

}
