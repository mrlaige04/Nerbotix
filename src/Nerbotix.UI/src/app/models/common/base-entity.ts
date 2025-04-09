import {IEntity} from './entity';
import {Guid} from 'guid-typescript';
import {IAuditableEntity} from './auditable-entity';

export abstract class BaseEntity implements IEntity<Guid>, IAuditableEntity {
  id!: Guid;
  createdAt!: Date;
  updatedAt?: Date;

  protected constructor(obj?: Partial<BaseEntity>) {
    if (obj) {
      Object.assign(this, obj);
    }
  }
}
