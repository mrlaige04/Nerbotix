import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {PaginationRequest} from '../../models/common/pagination-request';
import {PaginatedList} from '../../models/common/paginated-list';
import {UserBase} from '../../models/users/user-base';
import {Observable} from 'rxjs';
import {CreateUserRequest} from '../../models/users/requests/create-user-request';
import {Success} from '../../models/success';
import {Guid} from 'guid-typescript';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'users';

  getUsers(data: PaginationRequest): Observable<PaginatedList<UserBase>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<UserBase>>(url, { ...data });
  }

  createUser(data: CreateUserRequest): Observable<Success> {
    const url = this.baseUrl;
    return this.base.post<CreateUserRequest, Success>(url, data);
  }

  deleteUser(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
