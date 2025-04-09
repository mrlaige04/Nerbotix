import {TenantEntity} from '../../common/base-tenant-entity';

export class RobotCustomProperty extends TenantEntity {
  name!: string;
  value!: any;

  constructor(obj?: Partial<RobotCustomProperty>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
