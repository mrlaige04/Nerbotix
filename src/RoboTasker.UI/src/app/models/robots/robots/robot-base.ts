import {TenantEntity} from '../../common/base-tenant-entity';
import {CategoryBase} from '../categories/category-base';
import {RobotStatus} from './robot-status';
import {RobotLocation} from './robot-location';

export class RobotBase extends TenantEntity {
  name!: string;
  category!: CategoryBase;
  status!: RobotStatus;
  location!: RobotLocation;

  constructor(obj?: Partial<RobotBase>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
