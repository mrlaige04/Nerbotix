import {TenantEntity} from '../../common/base-tenant-entity';
import {TaskRequirementLevel} from './task-requirement-level';
import {Guid} from 'guid-typescript';

export class TaskRequirement extends TenantEntity {
  level!: TaskRequirementLevel;
  capabilityId!: Guid;
}
