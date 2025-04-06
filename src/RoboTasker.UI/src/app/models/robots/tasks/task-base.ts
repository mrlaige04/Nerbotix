import {TenantEntity} from '../../common/base-tenant-entity';
import {TaskStatus} from './task-status';
import {Guid} from 'guid-typescript';

export class TaskBase extends TenantEntity {
  name!: string;
  description?: string;
  status!: TaskStatus;
  completedAt?: Date;
  assignedRobotId?: Guid;
  estimatedDuration?: Date;
  priority!: number;
  complexity!: number;
  categoryId!: Guid;
}
