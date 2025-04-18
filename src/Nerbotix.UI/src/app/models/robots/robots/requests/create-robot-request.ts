import {Guid} from 'guid-typescript';
import {CommunicationType} from '../../../../enums/communication-type.enum';
import {HttpMethod} from '../../../../enums/http.method';

export interface CreateRobotRequest {
  name: string;
  categoryId: string; // GUID
  properties: CreateRobotPropertyRequest[];
  customProperties?: CreateRobotCustomPropertyRequest[] | undefined;
  capabilities?: CreateRobotCapabilityRequest[] | undefined;
  communicationType: CommunicationType;
  httpCommunication?: CreateRobotHttpCommunicationRequest;
  mqttCommunication?: CreateRobotMqttCommunicationRequest;
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

export interface CreateRobotHttpCommunicationRequest {
  url: string;
  method: HttpMethod;
  headers: Record<string, string>;
}

export interface CreateRobotMqttCommunicationRequest {
  mqttBrokerAddress: string;
  mqttBrokerUsername: string;
  mqttBrokerPassword: string;
  mqttTopic: string;
}
