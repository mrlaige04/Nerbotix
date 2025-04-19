import {CategoryPropertyType} from './category-property-type';
import {TenantEntity} from '../../common/base-tenant-entity';

export class CategoryProperty extends TenantEntity {
  name!: string;
  type!: CategoryPropertyType;
  factor!: number;
  unit?: string;

  constructor(obj?: Partial<CategoryProperty>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
