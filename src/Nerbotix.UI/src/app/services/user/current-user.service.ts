import {inject, Injectable, signal} from '@angular/core';
import {CurrentUser} from '../../models/user/current-user';
import {Observable, tap} from 'rxjs';
import {BaseHttp} from '../base/base-http';
import {Success} from '../../models/success';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'user';

  private readonly storageKey = 'current-user';
  private _currentUser = signal<CurrentUser | null>(this.getSavedUser());
  public currentUser = this._currentUser.asReadonly();

  private getSavedUser() {
    const json = localStorage.getItem(this.storageKey);
    if (json) {
      const token = JSON.parse(json) as CurrentUser;
      if (token) {
        return token;
      }
    }

    return null;
  }

  getCurrentUser(): Observable<CurrentUser> {
    const url = this.baseUrl + '/current-user';
    return this.base.get<CurrentUser>(url).pipe(
      tap(user => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  setCurrentUser(currentUser: CurrentUser) {
    localStorage.setItem(this.storageKey, JSON.stringify(currentUser));
    this._currentUser.set(currentUser);
  }

  uploadAvatar(file: File): Observable<Success> {
    const url = `${this.baseUrl}/profile-picture`;
    const formData = new FormData();
    formData.set('file', file);

    return this.base.put<FormData, string>(url, formData);
  }

  deleteAvatar(): Observable<Success> {
    const url = `${this.baseUrl}/profile-picture`;
    return this.base.delete<Success>(url);
  }

  clearCurrentUser() {
    localStorage.removeItem(this.storageKey);
    this._currentUser.set(null);
  }
}
