import {TenantEntity} from '../common/base-tenant-entity';

export class UserBase extends TenantEntity {
  email!: string;
  username!: string;
  emailVerified!: boolean;

  constructor(obj?: Partial<UserBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
