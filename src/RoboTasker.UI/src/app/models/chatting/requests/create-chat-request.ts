import {Guid} from 'guid-typescript';

export interface CreateChatRequest {
  name?: string;
  users: Guid[];
}
