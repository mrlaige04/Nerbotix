import {TenantEntity} from '../../common/base-tenant-entity';

export class RoleBase extends TenantEntity {
  name!: string;

  constructor(obj?: Partial<RoleBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
