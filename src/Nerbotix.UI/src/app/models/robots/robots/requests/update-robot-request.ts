import {Guid} from 'guid-typescript';
import {
  CreateRobotCapabilityRequest,
  CreateRobotCustomPropertyRequest,
  CreateRobotHttpCommunicationRequest, CreateRobotMqttCommunicationRequest
} from './create-robot-request';
import {CommunicationType} from '../../../../enums/communication-type.enum';

export interface UpdateRobotRequest {
  name?: string | undefined;
  categoryId?: string | undefined;
  updatedProperties?: UpdateRobotPropertyRequest[] | undefined;
  deletedCustomProperties?: Guid[] | undefined;
  newCustomProperties?: CreateRobotCustomPropertyRequest[] | undefined;
  deletedCapabilities?: CreateRobotCapabilityRequest[] | undefined;
  newCapabilities?: CreateRobotCapabilityRequest[] | undefined;
  communicationType?: CommunicationType;
  httpCommunication?: CreateRobotHttpCommunicationRequest;
  mqttCommunication?: CreateRobotMqttCommunicationRequest;
}

interface UpdateRobotPropertyRequest {
  propertyId: Guid;
  value: any;
}
