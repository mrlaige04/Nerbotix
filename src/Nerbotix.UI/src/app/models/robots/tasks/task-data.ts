import {TenantEntity} from '../../common/base-tenant-entity';
import {TaskDataType} from './task-data-type';

export class TaskData extends TenantEntity {
  key!: string;
  type!: TaskDataType;
  value?: string;
  fileName?: string;
}
