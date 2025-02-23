import {Guid} from 'guid-typescript';

export interface CurrentUser {
  email: string;
  tenantId: Guid;
  id: Guid;
}
