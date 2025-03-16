import {Routes} from '@angular/router';
import {RobotsListComponent} from './robots-list/robots-list.component';
import {RobotAddOrUpdateComponent} from './robot-add-or-update/robot-add-or-update.component';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';
import {hasPermissionGuard} from '../../../utils/guards/has-permission.guard';

export const ROBOTS_ROUTES: Routes = [
  {
    path: '',
    component: RobotsListComponent,
    canActivate: [hasPermissionGuard],
    title: 'Robots',
    data: {
      permission: PermissionsNames.RobotsRead
    }
  },
  {
    path: 'add',
    component: RobotAddOrUpdateComponent,
    title: 'Add Robot',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.RobotsCreate
    }
  },
  {
    path: ':id',
    component: RobotAddOrUpdateComponent,
    title: 'Edit robot',
    canActivate: [hasPermissionGuard],
    data: {
      permission: PermissionsNames.RobotsUpdate
    }
  }
];
