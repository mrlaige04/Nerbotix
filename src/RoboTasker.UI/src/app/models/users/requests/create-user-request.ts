import {Guid} from 'guid-typescript';

export interface CreateUserRequest {
  email: string;
  username?: string | undefined;
  roles: Guid[];
}
