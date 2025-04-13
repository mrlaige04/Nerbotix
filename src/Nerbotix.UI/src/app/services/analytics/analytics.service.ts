import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {Observable} from 'rxjs';
import {StatusesAnalytics} from '../../models/analytics/statuses-analytic';
import {MonthTasksCountAnalytic} from '../../models/analytics/month-tasks-count-analytic';
import {PaginationRequest} from '../../models/common/pagination-request';
import {PaginatedList} from '../../models/common/paginated-list';
import {TaskBase} from '../../models/robots/tasks/task-base';
import {RobotCountByCategoriesAnalytic} from '../../models/analytics/robot-count-by-categories-analytic';

@Injectable({
  providedIn: 'root'
})
export class AnalyticsService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'analytics';

  getTaskStatuses(): Observable<StatusesAnalytics> {
    const url = `${this.baseUrl}/tasks-statuses`;
    return this.base.get<StatusesAnalytics>(url);
  }

  getRobotStatuses(): Observable<StatusesAnalytics> {
    const url = `${this.baseUrl}/robot-statuses`;
    return this.base.get<StatusesAnalytics>(url);
  }

  getMonthTasksCreatedCount(): Observable<MonthTasksCountAnalytic> {
    const url = `${this.baseUrl}/month-tasks-created`;
    return this.base.get<MonthTasksCountAnalytic>(url);
  }

  getActiveTasks(request: PaginationRequest): Observable<PaginatedList<TaskBase>> {
    const url = `${this.baseUrl}/active-tasks`;
    return this.base.get<PaginatedList<TaskBase>>(url, { ...request });
  }

  getRobotCountByCategories(): Observable<RobotCountByCategoriesAnalytic> {
    const url = `${this.baseUrl}/robot-by-categories`;
    return this.base.get<RobotCountByCategoriesAnalytic>(url);
  }
}
