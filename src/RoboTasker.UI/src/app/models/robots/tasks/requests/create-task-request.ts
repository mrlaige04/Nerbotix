import {TaskRequirementLevel} from '../task-requirement-level';
import {TaskDataType} from '../task-data-type';

export interface CreateTaskRequest {
  name: string;
  description?: string;
  estimatedDuration: string;
  priority: number;
  complexity: number;
  properties?: CreateTaskPropertyRequest[];
  requirements?: CreateTaskRequirementRequest[];
  data?: CreateTaskDataRequest[];
  files?: File[];
}

export interface CreateTaskPropertyRequest {
  key: string;
  value: string;
}

export interface CreateTaskRequirementRequest {
  capabilityId: string;
  level: TaskRequirementLevel;
}

export interface CreateTaskDataRequest {
  key: string;
  type: TaskDataType;
  value?: any;
}
