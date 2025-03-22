import {Routes} from '@angular/router';
import {hasRoleGuard} from '../../utils/guards/has-role.guard';
import {RoleNames} from '../../models/tenants/roles/roles-names';
import {SaTenantsWrapperComponent} from './tenants/sa-tenants-wrapper/sa-tenants-wrapper.component';

export const SA_ROUTES: Routes = [
  {
    path: 'tenants',
    title: 'Tenants',
    canActivate: [hasRoleGuard],
    component: SaTenantsWrapperComponent,
    data: {
      role: RoleNames.SuperAdmin
    },
    loadChildren: () => import('./tenants/tenants.routes').then(r => r.TENANT_ROUTES)
  }
];
