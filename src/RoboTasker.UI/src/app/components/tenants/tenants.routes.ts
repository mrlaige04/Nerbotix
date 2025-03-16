import {Routes} from '@angular/router';
import {RolesListComponent} from './roles/roles-list/roles-list.component';
import {PermissionsListComponent} from './permissions/permissions-list/permissions-list.component';
import {RolesAddOrUpdateComponent} from './roles/roles-add-or-update/roles-add-or-update.component';
import {
  PermissionsAddOrUpdateComponent
} from './permissions/permissions-add-or-update/permissions-add-or-update.component';
import {hasPermissionGuard} from '../../utils/guards/has-permission.guard';
import {PermissionsNames} from '../../models/tenants/permissions/permissions-names';
import {UsersListComponent} from '../users/users-list/users-list.component';
import {UserEditComponent} from '../users/user-edit/user-edit.component';

export const TENANTS_ROUTES: Routes = [
  {
    path: 'roles',
    canActivate: [hasPermissionGuard],
    title: 'Roles',
    data: {
      permission: PermissionsNames.RolesRead
    },
    children: [
      {
        path: '',
        component: RolesListComponent,
        title: 'Roles',
        canActivate: [hasPermissionGuard],
        data: {
          permission: PermissionsNames.RolesRead
        },
      },
      {
        path: 'add',
        title: 'Add Role',
        component: RolesAddOrUpdateComponent,
        canActivate: [hasPermissionGuard],
        data: {
          permission: PermissionsNames.RolesCreate
        }
      },
      {
        path: ':id',
        title: 'Edit Role',
        component: RolesAddOrUpdateComponent,
        canActivate: [hasPermissionGuard],
        data: {
          permission: PermissionsNames.RolesUpdate
        }
      },
    ]
  },
  {
    path: 'permissions',
    canActivate: [hasPermissionGuard],
    title: 'Permissions',
    data: {
      permission: PermissionsNames.PermissionsRead
    },
    children: [
      {
        path: '',
        component: PermissionsListComponent ,
        title: 'Permissions',
        canActivate: [hasPermissionGuard],
        data: {
          permission: PermissionsNames.PermissionsRead
        },
      },
      {
        path: 'add',
        component: PermissionsAddOrUpdateComponent,
        canActivate: [hasPermissionGuard],
        title: 'Add permission',
        data: {
          permission: PermissionsNames.PermissionsCreate
        },
      },
      {
        path: ':id',
        component: PermissionsAddOrUpdateComponent,
        title: 'Edit permission',
        canActivate: [hasPermissionGuard],
        data: {
          permission: PermissionsNames.PermissionsUpdate
        },
      },
    ],
  },
  {
    path: 'users',
    canActivate: [hasPermissionGuard],
    title: 'Users',
    data: {
      permission: PermissionsNames.UsersRead
    },
    children: [
      {
        path: '',
        component: UsersListComponent,
        title: 'Users',
        canActivate: [hasPermissionGuard],
        data: {
          permission: PermissionsNames.UsersRead
        }
      },
      {
        path: ':id',
        component: UserEditComponent,
        title: 'Edit User',
        canActivate: [hasPermissionGuard],
        data: {
          permission: PermissionsNames.UsersUpdate
        }
      },
    ]
  }
];
