import {BaseEntity} from '../../common/base-entity';

export class TenantBase extends BaseEntity {
  email!: string;
  name!: string;

  constructor(obj?: Partial<TenantBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
