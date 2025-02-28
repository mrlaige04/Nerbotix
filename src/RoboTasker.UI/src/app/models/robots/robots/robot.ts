import {RobotBase} from './robot-base';
import {RobotPropertyValue} from './robot-property-value';
import {RobotCustomProperty} from './robot-custom-property';

export class Robot extends RobotBase {
  properties!: RobotPropertyValue[];
  customProperties?: RobotCustomProperty[] | undefined;

  constructor(obj?: Partial<Robot>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
