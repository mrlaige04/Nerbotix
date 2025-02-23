import {Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {
  private _sidebarOpened = signal(true);
  public sidebarOpened = this._sidebarOpened.asReadonly();

  toggleSidebar() {
    this._sidebarOpened.update(o => !o);
  }
}
