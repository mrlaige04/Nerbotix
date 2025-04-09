import {Routes} from '@angular/router';
import {TasksListComponent} from './tasks-list/tasks-list.component';
import {TasksAddOrUpdateComponent} from './tasks-add-or-update/tasks-add-or-update.component';
import {hasPermissionGuard} from '../../../utils/guards/has-permission.guard';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';

export const TASKS_ROUTES: Routes = [
  {
    path: '',
    component: TasksListComponent,
    title: 'Tasks',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.TasksRead,
    }
  },
  {
    path: 'add',
    component: TasksAddOrUpdateComponent,
    title: 'Add Task',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.TasksCreate,
    }
  },
  {
    path: ':id',
    component: TasksAddOrUpdateComponent,
    title: 'Edit Task',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.TasksUpdate,
    }
  },
];
