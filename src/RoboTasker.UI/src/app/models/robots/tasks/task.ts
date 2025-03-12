import {TaskBase} from './task-base';
import {TaskProperty} from './task-property';
import {TaskRequirement} from './task-requirement';
import { TaskData } from './task-data';

export class Task extends TaskBase {
  properties?: TaskProperty[];
  requirements?: TaskRequirement[];
  data?: TaskData[];
}
