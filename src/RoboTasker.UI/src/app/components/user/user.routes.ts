import {Routes} from '@angular/router';
import {ProfileComponent} from './profile/profile.component';
import {SettingsComponent} from './settings/settings.component';
import {ChangePasswordComponent} from './change-password/change-password.component';

export const USER_ROUTES: Routes = [
  {
    path: 'profile',
    component: ProfileComponent,
    title: 'Profile',
  },
  {
    path: 'change-password',
    component: ChangePasswordComponent,
    title: 'Change password',
  },
  {
    path: 'settings',
    component: SettingsComponent,
    title: 'Settings'
  }
];
