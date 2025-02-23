import {Routes} from '@angular/router';
import {ProfileComponent} from './profile/profile.component';
import {SettingsComponent} from './settings/settings.component';

export const USER_ROUTES: Routes = [
  { path: 'profile', component: ProfileComponent },
  { path: 'settings', component: SettingsComponent }
];
