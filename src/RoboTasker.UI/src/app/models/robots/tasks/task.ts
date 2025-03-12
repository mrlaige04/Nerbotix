import {TaskBase} from './task-base';
import {TaskProperty} from './task-property';
import {TaskRequirement} from './task-requirement';
import { TaskData } from './task-data';
import {TaskFile} from './task-file';

export class Task extends TaskBase {
  properties?: TaskProperty[];
  requirements?: TaskRequirement[];
  data?: TaskData[];
  files?: TaskFile[];
}
