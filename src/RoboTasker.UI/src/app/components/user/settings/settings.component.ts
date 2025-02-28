import {Component, inject} from '@angular/core';
import {Checkbox} from 'primeng/checkbox';
import {UiSettingsService} from '../../../services/layout/ui-settings.service';
import {ToggleSwitch} from 'primeng/toggleswitch';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'rb-settings',
  imports: [
    Checkbox,
    ToggleSwitch,
    FormsModule
  ],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent {
  private uiSettingsService = inject(UiSettingsService);
  theme = this.uiSettingsService.theme;

  toggleTheme() {
    this.uiSettingsService.toggleTheme();
  }
}
