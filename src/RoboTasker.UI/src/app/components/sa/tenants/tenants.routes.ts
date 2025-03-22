import { Routes} from '@angular/router';
import {hasRoleGuard} from '../../../utils/guards/has-role.guard';
import {RoleNames} from '../../../models/tenants/roles/roles-names';
import {TenantsListComponent} from './tenants-list/tenants-list.component';

export const TENANT_ROUTES: Routes = [
  {
    path: '',
    canActivate: [hasRoleGuard],
    title: 'Tenants',
    component: TenantsListComponent,
    data: {
      role: RoleNames.SuperAdmin
    }
  }
];
