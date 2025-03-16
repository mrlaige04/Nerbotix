import {TenantEntity} from '../../common/base-tenant-entity';

export class RoleBase extends TenantEntity {
  name!: string;
  isSystem!: boolean;

  constructor(obj?: Partial<RoleBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
