import {Routes} from '@angular/router';
import {LoginComponent} from './login/login.component';
import {inject} from '@angular/core';
import {AuthService} from '../../services/auth/auth.service';
import {ForgotComponent} from './forgot/forgot.component';

export const AUTH_ROUTES: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'forgot', component: ForgotComponent },
  { path: 'logout',
    redirectTo: () => {
      const authService = inject(AuthService);
      authService.logout();
      return '/';
    }
  }
];
