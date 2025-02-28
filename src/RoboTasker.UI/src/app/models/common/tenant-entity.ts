import {Guid} from 'guid-typescript';
import {IEntity} from './entity';

export interface ITenantEntity extends IEntity<Guid> {
  tenantId: Guid;
}
