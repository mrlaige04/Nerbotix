import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {CreatePermissionRequest} from '../../models/tenants/permissions/requests/create-permission-request';
import {Observable} from 'rxjs';
import {PermissionGroupBase} from '../../models/tenants/permissions/permission-group-base';
import {Guid} from 'guid-typescript';
import {Success} from '../../models/success';
import {PaginationRequest} from '../../models/common/pagination-request';
import {PaginatedList} from '../../models/common/paginated-list';
import {PermissionGroup} from '../../models/tenants/permissions/permission-group';
import {UpdatePermissionRequest} from '../../models/tenants/permissions/requests/update-permission-request';

@Injectable({
  providedIn: 'root'
})
export class PermissionsService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'permissions';

  createGroup(data: CreatePermissionRequest): Observable<PermissionGroupBase> {
    const url = this.baseUrl;
    return this.base.post<CreatePermissionRequest, PermissionGroupBase>(url, data);
  }

  getGroups(data: PaginationRequest) : Observable<PaginatedList<PermissionGroupBase>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<PermissionGroupBase>>(url, { ...data });
  }

  getGroup(id: Guid): Observable<PermissionGroup> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<PermissionGroup>(url);
  }

  updateGroup(id: Guid, data: UpdatePermissionRequest) : Observable<PermissionGroupBase> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.put<UpdatePermissionRequest, PermissionGroupBase>(url, data);
  }

  deleteGroup(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
