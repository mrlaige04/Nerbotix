import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {PaginationRequest} from '../../models/common/pagination-request';
import {PaginatedList} from '../../models/common/paginated-list';
import {UserBase} from '../../models/users/user-base';
import {Observable} from 'rxjs';
import {CreateUserRequest} from '../../models/users/requests/create-user-request';
import {Success} from '../../models/success';
import {Guid} from 'guid-typescript';
import {User} from '../../models/users/user';
import {UpdateUserRequest} from '../../models/users/requests/update-user-request';

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

  getUser(id: Guid): Observable<User> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<User>(url);
  }

  createUser(data: CreateUserRequest): Observable<Success> {
    const url = this.baseUrl;
    return this.base.post<CreateUserRequest, Success>(url, data);
  }

  updateUser(id: Guid, data: UpdateUserRequest): Observable<UserBase> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.put<UpdateUserRequest, UserBase>(url, data);
  }

  deleteUser(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
