import {TenantEntity} from '../../common/base-tenant-entity';

export class CapabilityItem extends TenantEntity {
  name!: string;
  description?: string;
  groupName!: string;

  constructor(obj?: Partial<CapabilityItem>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
