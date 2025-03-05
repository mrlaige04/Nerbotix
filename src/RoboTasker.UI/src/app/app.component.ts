import {Component, inject} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {GlobalLoaderComponent} from './components/common/global-loader/global-loader.component';
import {Toast} from 'primeng/toast';
import {ConfirmPopup} from 'primeng/confirmpopup';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {UiSettingsService} from './services/layout/ui-settings.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, GlobalLoaderComponent, Toast, ConfirmPopup, ConfirmDialog],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  private settingsService = inject(UiSettingsService);
  title = 'RoboTasker';
}
