import {RoleBase} from './roles/role-base';
import {PermissionBase} from './permissions/permission-base';

export class Role extends RoleBase {
  permissions!: PermissionBase[];

  constructor(obj?: Partial<Role>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
