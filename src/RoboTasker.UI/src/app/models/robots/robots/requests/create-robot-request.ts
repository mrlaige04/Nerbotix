import {Guid} from 'guid-typescript';

export interface CreateRobotRequest {
  name: string;
  categoryId: string; // GUID
  properties: CreateRobotPropertyRequest[];
  customProperties?: CreateRobotCustomPropertyRequest[] | undefined;
}

export interface CreateRobotPropertyRequest {
  propertyId: Guid;
  value: any;
}

export interface CreateRobotCustomPropertyRequest {
  name: string;
  value: any;
}
