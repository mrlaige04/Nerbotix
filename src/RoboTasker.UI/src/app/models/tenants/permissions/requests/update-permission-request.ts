import {Guid} from 'guid-typescript';
import {CreatePermissionRequestItem} from './create-permission-request';

export interface UpdatePermissionRequest {
  name?: string;
  deletePermissions?: Guid[];
  permissions?: CreatePermissionRequestItem[];
}
