import {TenantEntity} from '../../common/base-tenant-entity';

export class CategoryBase extends TenantEntity {
  name!: string;

  constructor(obj?: Partial<CategoryBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
