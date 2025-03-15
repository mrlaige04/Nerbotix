import {Guid} from 'guid-typescript';
import {PermissionBase} from '../tenants/permissions/permission-base';

export interface CurrentUser {
  email: string;
  tenantId: Guid;
  id: Guid;
  permissions: PermissionBase[];
}
