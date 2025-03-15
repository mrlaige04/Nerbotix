import {Routes} from '@angular/router';
import {RolesListComponent} from './roles/roles-list/roles-list.component';
import {PermissionsListComponent} from './permissions/permissions-list/permissions-list.component';
import {RolesAddOrUpdateComponent} from './roles/roles-add-or-update/roles-add-or-update.component';
import {
  PermissionsAddOrUpdateComponent
} from './permissions/permissions-add-or-update/permissions-add-or-update.component';

export const TENANTS_ROUTES: Routes = [
  { path: 'roles', component: RolesListComponent },
  { path: 'roles/add', component: RolesAddOrUpdateComponent },
  { path: 'roles/:id', component: RolesAddOrUpdateComponent },
  { path: 'permissions', component: PermissionsListComponent },
  { path: 'permissions/add', component: PermissionsAddOrUpdateComponent },
  { path: 'permissions/:id', component: PermissionsAddOrUpdateComponent },
];
