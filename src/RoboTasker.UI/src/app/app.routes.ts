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
import {TenantsWrapperComponent} from './components/tenants/tenants-wrapper/tenants-wrapper.component';
import {ForbiddenComponent} from './components/layout/forbidden/forbidden.component';
import {SaWrapperComponent} from './components/sa/sa-wrapper/sa-wrapper.component';
import {isSuperAdminGuard} from './utils/guards/is-super-admin.guard';

export const routes: Routes = [
  {
    path: '',
    title: 'Welcome to RoboTasker',
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
        canActivate: [isAuthenticatedGuard],
        title: 'User',
        loadChildren: () =>
          import('./components/user/user.routes').then(r => r.USER_ROUTES)
      },
      {
        path: 'categories',
        title: 'Categories',
        component: CategoriesWrapperComponent,
        loadChildren: () =>
          import('./components/robots/categories/categories.routes').then(r => r.CATEGORIES_ROUTES),
      },
      {
        path: 'robots',
        title: 'Robots',
        component: RobotsWrapperComponent,
        loadChildren: () =>
          import('./components/robots/robots/robots.routes').then(r => r.ROBOTS_ROUTES)
      },
      {
        path: 'capabilities',
        title: 'Capabilities',
        component: CapabilitiesWrapperComponent,
        loadChildren: () =>
          import('./components/robots/capabilities/capabilities.routes').then(r => r.CAPABILITIES_ROUTES)
      },
      {
        path: 'tasks',
        title: 'Tasks',
        component: TasksWrapperComponent,
        loadChildren: () =>
          import('./components/robots/tasks/tasks.routes').then(r => r.TASKS_ROUTES)
      },
      {
        path: 'tenant',
        component: TenantsWrapperComponent,
        loadChildren: () =>
          import('./components/tenants/tenants.routes').then(r => r.TENANTS_ROUTES)
      },
      {
        path: 'chats',
        loadChildren: () =>
          import('./components/chats/chat.routes').then(r => r.CHATS_ROUTES)
      },
      {
        path: 'sa',
        component: SaWrapperComponent,
        loadChildren: () =>
          import('./components/sa/sa.routes').then(r => r.SA_ROUTES),
        canActivate: [isSuperAdminGuard]
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
  { path: 'no-access', component: ForbiddenComponent }
];
