import {TaskRequirementLevel} from '../task-requirement-level';
import {TaskDataType} from '../task-data-type';
import {Guid} from 'guid-typescript';

export interface CreateTaskRequest {
  name: string;
  description?: string;
  estimatedDuration: string;
  priority: number;
  categoryId?: Guid;
  complexity: number;
  requirements?: CreateTaskRequirementRequest[];
  data?: CreateTaskDataRequest[];
  files?: File[];
}

export interface CreateTaskRequirementRequest {
  capabilityId: string;
  level: TaskRequirementLevel;
  existingId?: Guid;
}

export interface CreateTaskDataRequest {
  key: string;
  type: TaskDataType;
  value?: any;
  existingId?: Guid;
}
