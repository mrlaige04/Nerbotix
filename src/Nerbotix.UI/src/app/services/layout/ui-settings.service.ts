import {effect, Injectable, signal} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UiSettingsService {
  private readonly storageKey = 'theme';
  private _theme = signal<'light' | 'dark'>(this.getStoredTheme());
  public theme = this._theme.asReadonly();

  constructor() {
    effect(() => {
      const theme = this.theme();
      if (theme === 'dark') {
        document.documentElement.classList.add('dark-theme');
      } else {
        document.documentElement.classList.remove('dark-theme');
      }
    });
  }

  toggleTheme() {
    this._theme.update(theme => {
      return theme === 'dark' ? 'light' : 'dark';
    });

    localStorage.setItem(this.storageKey, this.theme());
  }

  private getStoredTheme() {
    const savedTheme = localStorage.getItem(this.storageKey);
    if (savedTheme === 'dark' || savedTheme === 'light') {
      return savedTheme;
    }

    return 'dark';
  }
}
