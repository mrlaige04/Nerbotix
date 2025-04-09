import {TenantEntity} from '../common/base-tenant-entity';
import {RoleBase} from '../tenants/roles/role-base';

export class UserBase extends TenantEntity {
  email!: string;
  username!: string;
  emailVerified!: boolean;
  roles!: RoleBase[];

  constructor(obj?: Partial<UserBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
