import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {PaginationRequest} from '../../models/common/pagination-request';
import {Observable} from 'rxjs';
import {PaginatedList} from '../../models/common/paginated-list';
import {TenantBase} from '../../models/super-admin/tenants/tenant-base';
import {CreateTenantRequest} from '../../models/super-admin/tenants/requests/create-tenant-request';
import {Guid} from 'guid-typescript';
import {Success} from '../../models/success';

@Injectable({
  providedIn: 'root'
})
export class TenantsService {
  private base = inject(BaseHttp);
  private baseUrl = 'sa';

  createTenant(data: CreateTenantRequest): Observable<TenantBase> {
    const url = `${this.baseUrl}/tenants`;
    return this.base.post<CreateTenantRequest, TenantBase>(url, data);
  }

  getTenants(data: PaginationRequest): Observable<PaginatedList<TenantBase>> {
    const url = `${this.baseUrl}/tenants`;
    return this.base.get<PaginatedList<TenantBase>>(url, { ...data });
  }

  deleteTenant(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/tenants/${id}`;
    return this.base.delete<Success>(url);
  }
}
