import {Routes} from '@angular/router';
import {CapabilitiesListComponent} from './capabilities-list/capabilities-list.component';
import {CapabilityAddOrUpdateComponent} from './capability-add-or-update/capability-add-or-update.component';
import {hasPermissionGuard} from '../../../utils/guards/has-permission.guard';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';

export const CAPABILITIES_ROUTES: Routes = [
  {
    path: '',
    component: CapabilitiesListComponent,
    title: 'Capabilities',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.CapabilitiesRead
    }
  },
  {
    path: 'add',
    component: CapabilityAddOrUpdateComponent,
    title: 'Add capability',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.CapabilitiesCreate
    }
  },
  {
    path: ':id',
    component: CapabilityAddOrUpdateComponent,
    title: 'Edit capability',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.CapabilitiesUpdate
    }
  }
];
