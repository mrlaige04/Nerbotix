import {Guid} from 'guid-typescript';

export interface UpdateUserRequest {
  username?: string,
  deleteRoles?: Guid[];
  roles?: Guid[];
}
