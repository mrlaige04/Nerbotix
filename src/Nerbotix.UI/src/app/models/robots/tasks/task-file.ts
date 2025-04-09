import {BaseEntity} from '../../common/base-entity';

export class TaskFile extends BaseEntity {
  fileName!: string;
  contentType!: string;
  size!: number;
}
