import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {CreateRoleRequest} from '../../models/tenants/roles/requests/create-role-request';
import {Observable} from 'rxjs';
import {RoleBase} from '../../models/tenants/roles/role-base';
import {PaginationRequest} from '../../models/common/pagination-request';
import {PaginatedList} from '../../models/common/paginated-list';
import {Guid} from 'guid-typescript';
import {Role} from '../../models/tenants/role';
import {Success} from '../../models/success';
import {UpdateRoleRequest} from '../../models/tenants/roles/requests/update-role-request';

@Injectable({
  providedIn: 'root'
})
export class RolesService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'roles';

  createRole(data: CreateRoleRequest) : Observable<RoleBase> {
    const url = this.baseUrl;
    return this.base.post<CreateRoleRequest, RoleBase>(url, data);
  }

  getRoles(data: PaginationRequest) : Observable<PaginatedList<RoleBase>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<RoleBase>>(url, { ...data });
  }

  getRole(id: Guid) : Observable<Role> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<Role>(url);
  }

  updateRole(id: Guid, data: UpdateRoleRequest) : Observable<RoleBase> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.put<UpdateRoleRequest, RoleBase>(url, data);
  }

  deleteRole(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
