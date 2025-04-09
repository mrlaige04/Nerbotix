import {CapabilityBase} from './capability-base';
import {CapabilityItem} from './capability-item';

export class Capability extends CapabilityBase {
  capabilities!: CapabilityItem[];

  constructor(obj?: Partial<Capability>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
