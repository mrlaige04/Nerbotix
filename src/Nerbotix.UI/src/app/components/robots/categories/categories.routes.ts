import {Routes} from '@angular/router';
import {CategoriesListComponent} from './categories-list/categories-list.component';
import {hasPermissionGuard} from '../../../utils/guards/has-permission.guard';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';

export const CATEGORIES_ROUTES: Routes = [
  {
    path: '',
    component: CategoriesListComponent,
    title: 'Categories',
    data: {
      permission: PermissionsNames.CategoriesRead
    },
    canActivate: [hasPermissionGuard],
  },
];
