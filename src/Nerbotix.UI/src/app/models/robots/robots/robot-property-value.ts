import {TenantEntity} from '../../common/base-tenant-entity';
import {CategoryPropertyType} from '../categories/category-property-type';
import {Guid} from 'guid-typescript';

export class RobotPropertyValue extends TenantEntity {
  propertyId!: Guid;
  value!: any;
  type!: CategoryPropertyType;

  constructor(obj?: Partial<RobotPropertyValue>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
