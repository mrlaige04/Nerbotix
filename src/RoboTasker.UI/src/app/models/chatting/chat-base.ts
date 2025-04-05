import {TenantEntity} from '../common/base-tenant-entity';

export class ChatBase extends TenantEntity {
  name!: string;
  lastMessage?: string;
}
