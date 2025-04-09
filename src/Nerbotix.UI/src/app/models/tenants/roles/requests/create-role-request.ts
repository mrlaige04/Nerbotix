import {Guid} from 'guid-typescript';

export interface CreateRoleRequest {
  name: string;
  permissions: Guid[];
}
