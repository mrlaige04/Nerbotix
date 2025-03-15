import {CreateTaskDataRequest, CreateTaskRequirementRequest} from './create-task-request';
import {Guid} from 'guid-typescript';

export interface UpdateTaskRequest {
  name?: string;
  description?: string;
  priority?: number;
  complexity?: number;
  estimatedDuration?: string;
  requirements?: CreateTaskRequirementRequest[];
  data?: CreateTaskDataRequest[];
  files?: File[];

  deletedRequirements?: Guid[] | undefined;
  deletedData?: Guid[] | undefined;
  deletedFiles?: string[] | undefined;
}
