import {TenantEntity} from '../../common/base-tenant-entity';

export class TaskProperty extends TenantEntity {
  key!: string;
  value!: string;
}
