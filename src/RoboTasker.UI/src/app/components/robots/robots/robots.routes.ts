import {Routes} from '@angular/router';
import {RobotsListComponent} from './robots-list/robots-list.component';
import {RobotAddOrUpdateComponent} from './robot-add-or-update/robot-add-or-update.component';

export const ROBOTS_ROUTES: Routes = [
  { path: '', component: RobotsListComponent, data: { title: 'Robots' } },
  { path: 'add', component: RobotAddOrUpdateComponent, },
  { path: ':id', component: RobotAddOrUpdateComponent }
];
