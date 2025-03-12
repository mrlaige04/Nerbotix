import { Routes } from '@angular/router';
import {LayoutComponent} from './components/layout/layout/layout.component';
import {AuthWrapperComponent} from './components/auth/auth-wrapper/auth-wrapper.component';
import {isNotAuthenticatedGuard} from './utils/guards/is-not-authenticated.guard';
import {isAuthenticatedGuard} from './utils/guards/is-authenticated.guard';
import {UserWrapperComponent} from './components/user/user-wrapper/user-wrapper.component';
import {
  CategoriesWrapperComponent
} from './components/robots/categories/categories-wrapper/categories-wrapper.component';
import {RobotsWrapperComponent} from './components/robots/robots/robots-wrapper/robots-wrapper.component';
import {MaintenanceComponent} from './components/layout/maintenance/maintenance.component';
import {
  CapabilitiesWrapperComponent
} from './components/robots/capabilities/capabilities-wrapper/capabilities-wrapper.component';
import {HomeComponent} from './components/home/home.component';
import {TasksWrapperComponent} from './components/robots/tasks/tasks-wrapper/tasks-wrapper.component';
import {UsersWrapperComponent} from './components/users/users-wrapper/users-wrapper.component';

export const routes: Routes = [
  {
    path: '',
    data: { title: 'Welcome to RoboTasker' },
    component: LayoutComponent,
    canActivate: [isAuthenticatedGuard],
    children: [
      {
        path: '',
        component: HomeComponent
      },
      {
        path: 'user',
        component: UserWrapperComponent,
        loadChildren: () =>
          import('./components/user/user.routes').then(r => r.USER_ROUTES)
      },
      {
        path: 'categories',
        data: { title: 'Categories' },
        component: CategoriesWrapperComponent,
        loadChildren: () =>
          import('./components/robots/categories/categories.routes').then(r => r.CATEGORIES_ROUTES)
      },
      {
        path: 'robots',
        data: { title: 'Robots' },
        component: RobotsWrapperComponent,
        loadChildren: () =>
          import('./components/robots/robots/robots.routes').then(r => r.ROBOTS_ROUTES)
      },
      {
        path: 'capabilities',
        data: { title: 'Capabilities' },
        component: CapabilitiesWrapperComponent,
        loadChildren: () =>
          import('./components/robots/capabilities/capabilities.routes').then(r => r.CAPABILITIES_ROUTES)
      },
      {
        path: 'tasks',
        data: { title: 'Tasks' },
        component: TasksWrapperComponent,
        loadChildren: () =>
          import('./components/robots/tasks/tasks.routes').then(r => r.TASKS_ROUTES)
      },
      {
        path: 'users',
        title: 'Users',
        component: UsersWrapperComponent,
        loadChildren: () =>
          import('./components/users/users-routes').then(r => r.USERS_ROUTES)
      }
    ]
  },
  {
    path: 'auth',
    canActivate: [isNotAuthenticatedGuard],
    component: AuthWrapperComponent,
    loadChildren: () =>
      import('./components/auth/auth.routes').then(r => r.AUTH_ROUTES)
  },
  { path: 'maintenance', component: MaintenanceComponent },
];
