import {Guid} from 'guid-typescript';

export interface CreateRobotRequest {
  name: string;
  categoryId: string; // GUID
  properties: CreateRobotPropertyRequest[];
  customProperties?: CreateRobotCustomPropertyRequest[] | undefined;
  capabilities?: CreateRobotCapabilityRequest[] | undefined;
}

export interface CreateRobotPropertyRequest {
  propertyId: Guid;
  value: any;
}

export interface CreateRobotCustomPropertyRequest {
  name: string;
  value: any;
}

export interface CreateRobotCapabilityRequest {
  groupId: Guid;
  id: Guid;
}
