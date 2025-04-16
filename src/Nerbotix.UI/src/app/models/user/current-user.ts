import {Guid} from 'guid-typescript';
import {PermissionBase} from '../tenants/permissions/permission-base';
import {RoleBase} from '../tenants/roles/role-base';

export interface CurrentUser {
  email: string;
  tenantId: Guid;
  tenantName: string;
  id: Guid;
  permissions: PermissionBase[];
  roles: RoleBase[];
  avatarUrl?: string;
}
