import {Routes} from '@angular/router';
import {UsersListComponent} from './users-list/users-list.component';
import {UserEditComponent} from './user-edit/user-edit.component';

export const USERS_ROUTES: Routes = [
  { path: '', component: UsersListComponent },
  { path: ':id', component: UserEditComponent },
];
