import {inject, Injectable, signal} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {LoginRequest} from '../../models/auth/login-request';
import {Observable} from 'rxjs';
import {AccessToken} from '../../models/auth/access-token';
import {ForgotPasswordRequest} from '../../models/auth/forgot-password-request';
import {Success} from '../../models/success';
import {ResetPasswordRequest} from '../../models/auth/reset-password-request';
import {ChangePasswordRequest} from '../../models/auth/change-password-request';
import {CurrentUserService} from '../user/current-user.service';
import {LoginResponse} from '../../models/auth/login-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserService = inject(CurrentUserService);
  private base = inject(BaseHttp);
  private readonly baseUrl = 'auth'
  private readonly storageKey = 'JWT';

  private _accessToken = signal<AccessToken | null>(this.getSavedToken());
  public accessToken = this._accessToken.asReadonly();

  private _isAuthenticated = signal(this.getSavedToken() !== null);
  public isAuthenticated = this._isAuthenticated.asReadonly();

  login(data: LoginRequest): Observable<LoginResponse> {
    const url = `${this.baseUrl}/login`;
    return this.base.post<LoginRequest, LoginResponse>(url, data);
  }

  forgotPassword(data: ForgotPasswordRequest): Observable<Success> {
    const url = `${this.baseUrl}/forgot-password`;
    return this.base.post<ForgotPasswordRequest, Success>(url, data);
  }

  resetPassword(data: ResetPasswordRequest): Observable<Success> {
    const url = `${this.baseUrl}/reset-password`;
    return this.base.post<ResetPasswordRequest, Success>(url, data);
  }

  changePassword(data: ChangePasswordRequest): Observable<Success> {
    const url = `${this.baseUrl}/change-password`;
    return this.base.post<ChangePasswordRequest, Success>(url, data);
  }

  handleSuccessLogin(result: LoginResponse) {
    localStorage.setItem(this.storageKey, JSON.stringify(result.token));

    this._accessToken.set(result.token);
    this._isAuthenticated.set(true);

    this.currentUserService.setCurrentUser(result.user);
  }

  private getSavedToken() {
    const json = localStorage.getItem(this.storageKey);
    if (json) {
      const token = JSON.parse(json) as AccessToken;
      if (token) {
        return token;
      }
    }

    return null;
  }

  logout() {
    localStorage.removeItem(this.storageKey);

    this._accessToken.set(null);
    this._isAuthenticated.set(false);

    this.currentUserService.clearCurrentUser();

    location.reload();
  }
}
