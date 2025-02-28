import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {PaginationRequest} from '../../models/common/pagination-request';
import {RobotBase} from '../../models/robots/robots/robot-base';
import {PaginatedList} from '../../models/common/paginated-list';
import {Observable} from 'rxjs';
import {Guid} from 'guid-typescript';
import {Robot} from '../../models/robots/robots/robot';
import {CreateRobotRequest} from '../../models/robots/robots/requests/create-robot-request';
import {Success} from '../../models/success';
import {UpdateRobotRequest} from '../../models/robots/robots/requests/update-robot-request';

@Injectable({
  providedIn: 'root'
})
export class RobotsService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'robots';

  getRobots(data: PaginationRequest): Observable<PaginatedList<RobotBase>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<RobotBase>>(url, { ...data });
  }

  getRobotById(id: Guid): Observable<Robot | null> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<Robot>(url);
  }

  createRobot(data: CreateRobotRequest): Observable<RobotBase> {
    const url = this.baseUrl;
    return this.base.post<CreateRobotRequest, RobotBase>(url, data);
  }

  updateRobot(id: Guid, data: UpdateRobotRequest): Observable<RobotBase> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.put<UpdateRobotRequest, RobotBase>(url, data);
  }

  deleteRobot(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
