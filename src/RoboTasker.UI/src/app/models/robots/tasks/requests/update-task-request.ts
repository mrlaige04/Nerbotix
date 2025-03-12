import {CreateTaskDataRequest, CreateTaskPropertyRequest, CreateTaskRequirementRequest} from './create-task-request';
import {Guid} from 'guid-typescript';

export interface UpdateTaskRequest {
  name?: string;
  description?: string;
  priority?: number;
  complexity?: number;
  estimatedDuration?: string;
  properties?: CreateTaskPropertyRequest[];
  requirements?: CreateTaskRequirementRequest[];
  data?: CreateTaskDataRequest[];
  files?: File[];

  deletedProperties?: Guid[] | undefined;
  deletedRequirements?: Guid[] | undefined;
  deletedData?: Guid[] | undefined;
  deletedFiles?: string[] | undefined;
}
