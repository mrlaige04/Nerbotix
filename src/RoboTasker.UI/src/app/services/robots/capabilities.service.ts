import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {PaginationRequest} from '../../models/common/pagination-request';
import {Observable} from 'rxjs';
import {PaginatedList} from '../../models/common/paginated-list';
import {CapabilityBase} from '../../models/robots/capabilities/capability-base';
import {Guid} from 'guid-typescript';
import {Capability} from '../../models/robots/capabilities/capability';
import {CreateCapabilityRequest} from '../../models/robots/capabilities/requests/create-capability-request';
import {UpdateCapabilityRequest} from '../../models/robots/capabilities/requests/update-capability-request';
import {Success} from '../../models/success';
import {CapabilityItem} from '../../models/robots/capabilities/capability-item';

@Injectable({
  providedIn: 'root'
})
export class CapabilitiesService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'capabilities';

  getCapabilitiesGroups(data: PaginationRequest): Observable<PaginatedList<CapabilityBase>> {
    const url = `${this.baseUrl}/groups`;
    return this.base.get<PaginatedList<CapabilityBase>>(url, { ...data });
  }

  getCapabilitiesItems(data: PaginationRequest): Observable<PaginatedList<CapabilityItem>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<CapabilityItem>>(url, { ...data });
  }

  getCapabilityById(id: Guid): Observable<Capability | null> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<Capability | null>(url);
  }

  createCapability(data: CreateCapabilityRequest): Observable<CapabilityBase> {
    const url = this.baseUrl;
    return this.base.post<CreateCapabilityRequest, CapabilityBase>(url, data);
  }

  updateCapability(id: Guid, data: UpdateCapabilityRequest): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.put<UpdateCapabilityRequest, CapabilityBase>(url, data);
  }

  deleteCapability(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
