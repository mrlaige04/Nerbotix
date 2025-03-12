import {Routes} from '@angular/router';
import {TasksListComponent} from './tasks-list/tasks-list.component';
import {TasksAddOrUpdateComponent} from './tasks-add-or-update/tasks-add-or-update.component';

export const TASKS_ROUTES: Routes = [
  { path: '', component: TasksListComponent },
  { path: 'add', component: TasksAddOrUpdateComponent },
  { path: ':id', component: TasksAddOrUpdateComponent },
];
