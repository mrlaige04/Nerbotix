import {TenantEntity} from '../../common/base-tenant-entity';

export class PermissionGroupBase extends TenantEntity {
  name!: string;

  constructor(obj?: Partial<PermissionGroupBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
