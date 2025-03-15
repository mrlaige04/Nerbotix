import {TaskBase} from './task-base';
import {TaskRequirement} from './task-requirement';
import { TaskData } from './task-data';
import {TaskFile} from './task-file';

export class Task extends TaskBase {
  requirements?: TaskRequirement[];
  data?: TaskData[];
  files?: TaskFile[];
}
