import {RobotBase} from './robot-base';
import {RobotPropertyValue} from './robot-property-value';
import {RobotCustomProperty} from './robot-custom-property';
import {RobotCapability} from './robot-capability';
import {RobotCommunication} from './robot-communication';
import { Log } from '../../logging/log';

export class Robot extends RobotBase {
  properties!: RobotPropertyValue[];
  customProperties?: RobotCustomProperty[] | undefined;
  capabilities?: RobotCapability[] | undefined;
  logs?: Log[];
  communication!: RobotCommunication;

  constructor(obj?: Partial<Robot>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
