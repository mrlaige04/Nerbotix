import {Injectable, signal} from '@angular/core';
import {CurrentUser} from '../../models/user/current-user';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {
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

  setCurrentUser(currentUser: CurrentUser) {
    localStorage.setItem(this.storageKey, JSON.stringify(currentUser));
    this._currentUser.set(currentUser);
  }

  clearCurrentUser() {
    localStorage.removeItem(this.storageKey);
    this._currentUser.set(null);
  }
}
