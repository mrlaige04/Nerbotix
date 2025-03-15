import {TenantEntity} from '../../common/base-tenant-entity';

export class PermissionBase extends TenantEntity {
  groupName!: string;
  name!: string;

  constructor(obj?: Partial<PermissionBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
