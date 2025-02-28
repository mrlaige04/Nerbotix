import {Routes} from '@angular/router';
import {CategoriesListComponent} from './categories-list/categories-list.component';

export const CATEGORIES_ROUTES: Routes = [
  { path: '', component: CategoriesListComponent, data: { title: 'Categories' } },
];
