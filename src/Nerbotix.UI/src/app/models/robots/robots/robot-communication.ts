import {CommunicationType} from '../../../enums/communication-type.enum';
import {HttpMethod} from '../../../enums/http.method';

export type RobotCommunication = {
  communicationType: CommunicationType;
} & (HttpCommunication | MqttCommunication);

export type HttpCommunication = {
  apiEndpoint: string;
  httpMethod: HttpMethod;
  headers?: Record<string, string>;
};

export type MqttCommunication = {
  mqttBrokerAddress: string;
  mqttBrokerUsername: string;
  mqttBrokerPassword: string;
  mqttTopic: string;
};
