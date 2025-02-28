import {TenantEntity} from '../../common/base-tenant-entity';
import {CategoryBase} from '../categories/category-base';

export class RobotBase extends TenantEntity {
  name!: string;
  category!: CategoryBase;

  constructor(obj?: Partial<RobotBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
