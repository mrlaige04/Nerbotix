import {Guid} from 'guid-typescript';
import {CreateRobotCustomPropertyRequest} from './create-robot-request';

export interface UpdateRobotRequest {
  name?: string | undefined;
  categoryId?: string | undefined;
  updatedProperties?: UpdateRobotPropertyRequest[] | undefined;
  deletedCustomProperties?: Guid[] | undefined;
  newCustomProperties?: CreateRobotCustomPropertyRequest[] | undefined;
}

interface UpdateRobotPropertyRequest {
  propertyId: Guid;
  value: any;
}
