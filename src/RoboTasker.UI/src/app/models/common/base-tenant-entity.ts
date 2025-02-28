import {BaseEntity} from './base-entity';
import {ITenantEntity} from './tenant-entity';
import {Guid} from 'guid-typescript';

export abstract class TenantEntity extends BaseEntity implements ITenantEntity {
  tenantId!: Guid;

  protected constructor(obj?: Partial<TenantEntity>) {
    super(obj);
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
