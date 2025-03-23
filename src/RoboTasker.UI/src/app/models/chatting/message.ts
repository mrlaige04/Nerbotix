import {TenantEntity} from '../common/base-tenant-entity';
import {Guid} from 'guid-typescript';

export class Message extends TenantEntity {
  isSystem!: boolean;
  message!: string;
  senderId?: Guid;
}
