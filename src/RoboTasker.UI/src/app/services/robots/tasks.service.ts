import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {PaginationRequest} from '../../models/common/pagination-request';
import {Observable} from 'rxjs';
import {PaginatedList} from '../../models/common/paginated-list';
import {TaskBase} from '../../models/robots/tasks/task-base';
import {Guid} from 'guid-typescript';
import {Task} from '../../models/robots/tasks/task';
import {CreateTaskRequest} from '../../models/robots/tasks/requests/create-task-request';
import {Success} from '../../models/success';

@Injectable({
  providedIn: 'root'
})
export class TasksService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'tasks';

  getTasks(data: PaginationRequest): Observable<PaginatedList<TaskBase>> {
    const url = this.baseUrl;
    return this.base.get<PaginatedList<TaskBase>>(url, { ...data });
  }

  getTaskById(id: Guid): Observable<Task | null> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.get<Task>(url);
  }

  createTask(data: CreateTaskRequest): Observable<TaskBase> {
    const url = this.baseUrl;
    const formData = new FormData();
    formData.append('name', data.name);
    formData.append('description', data.description ?? '');
    formData.append('estimatedDuration', data.estimatedDuration);
    formData.append('priority', data.priority.toString());
    formData.append('complexity', data.complexity.toString());
    formData.append('properties', JSON.stringify(data.properties));
    formData.append('requirements', JSON.stringify(data.requirements));
    formData.append('data', JSON.stringify(data.data));

    if (data.files) {
      data.files.forEach(file => {
        formData.append('files', file, file.name);
      });
    }

    return this.base.post<FormData, TaskBase>(url, formData);
  }

  deleteTask(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
