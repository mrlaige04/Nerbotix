import {Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GlobalLoaderService {
  private _isShowLoader = signal(false);
  public isShowLoader = this._isShowLoader.asReadonly();

  showLoader() { this._isShowLoader.set(true); }
  hideLoader() { this._isShowLoader.set(false); }
}
