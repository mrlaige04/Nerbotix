import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {PaginationRequest} from '../../models/common/pagination-request';
import {Category} from '../../models/robots/categories/category';
import {PaginatedList} from '../../models/common/paginated-list';
import {Observable} from 'rxjs';
import {CreateCategoryRequest} from '../../models/robots/categories/requests/create-category-request';
import {Success} from '../../models/success';
import {Guid} from 'guid-typescript';
import {CategoryBase} from '../../models/robots/categories/category-base';
import {UpdateCategoryRequest} from '../../models/robots/categories/requests/update-category-request';
import {
  UpdateCategoryLinearOptimizationParamsRequest
} from '../../models/robots/categories/requests/update-category-linearopt-params-request';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'categories';

  getCategories(data: PaginationRequest): Observable<PaginatedList<CategoryBase>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<Category>>(url, { ...data });
  }

  getCategoryById(id: Guid): Observable<Category | null> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<Category | null>(url);
  }

  createCategory(data: CreateCategoryRequest): Observable<CategoryBase> {
    const url = this.baseUrl;
    return this.base.post<CreateCategoryRequest, CategoryBase>(url, data);
  }

  updateCategory(id: Guid, data: UpdateCategoryRequest): Observable<CategoryBase> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.put<UpdateCategoryRequest, CategoryBase>(url, data);
  }

  deleteCategory(id: Guid) : Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }

  updateLinearOptimizationAlgoParams(id: Guid, data: UpdateCategoryLinearOptimizationParamsRequest): Observable<Success> {
    const url = `${this.baseUrl}/${id}/algorithms/linear-optimization`;
    return this.base.put<UpdateCategoryLinearOptimizationParamsRequest, Success>(url, data);
  }
}
