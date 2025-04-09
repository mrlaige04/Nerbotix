import {UserBase} from './user-base';

export class User extends UserBase {
  constructor(obj?: Partial<User>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
