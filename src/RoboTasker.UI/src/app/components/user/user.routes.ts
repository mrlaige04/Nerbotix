import {Routes} from '@angular/router';
import {ProfileComponent} from './profile/profile.component';
import {SettingsComponent} from './settings/settings.component';
import {ChangePasswordComponent} from './change-password/change-password.component';

export const USER_ROUTES: Routes = [
  { path: 'profile', component: ProfileComponent },
  { path: 'change-password', component: ChangePasswordComponent },
  { path: 'settings', component: SettingsComponent }
];
