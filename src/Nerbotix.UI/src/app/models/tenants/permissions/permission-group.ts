import {PermissionGroupBase} from './permission-group-base';
import {PermissionBase} from './permission-base';

export class PermissionGroup extends PermissionGroupBase {
  permissions!: PermissionBase[];

  constructor(obj?: Partial<PermissionGroup>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
