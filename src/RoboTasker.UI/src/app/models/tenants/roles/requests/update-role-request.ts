import {Guid} from 'guid-typescript';

export interface UpdateRoleRequest {
  name?: string;
  deletePermissions?: Guid[];
  permissions?: Guid[];
}
