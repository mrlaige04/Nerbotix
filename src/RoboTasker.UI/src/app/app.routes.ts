import { Routes } from '@angular/router';
import {LayoutComponent} from './components/layout/layout/layout.component';
import {AuthWrapperComponent} from './components/auth/auth-wrapper/auth-wrapper.component';
import {isNotAuthenticatedGuard} from './utils/guards/is-not-authenticated.guard';
import {isAuthenticatedGuard} from './utils/guards/is-authenticated.guard';

export const routes: Routes = [
  { path: '', component: LayoutComponent, canActivate: [isAuthenticatedGuard], children: [
      { path: 'user', loadChildren: () => import('./components/user/user.routes').then(r => r.USER_ROUTES) },
    ] },
  { path: 'auth', canActivate: [isNotAuthenticatedGuard], component: AuthWrapperComponent, loadChildren: () =>
      import('./components/auth/auth.routes').then(r => r.AUTH_ROUTES) }
];
