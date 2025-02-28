import {inject, Injectable, signal} from '@angular/core';
import {DeviceDetectorService} from 'ngx-device-detector';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {
  private deviceDetectorService = inject(DeviceDetectorService);
  private _sidebarOpened = signal(this.deviceDetectorService.isDesktop());
  public sidebarOpened = this._sidebarOpened.asReadonly();

  toggleSidebar() {
    this._sidebarOpened.update(o => !o);
  }

  get isDesktop() {
    return this.deviceDetectorService.isDesktop();
  }

  closeSidebar() {
    this._sidebarOpened.set(false);
  }
}
