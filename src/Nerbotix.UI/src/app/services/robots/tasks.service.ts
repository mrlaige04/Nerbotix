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
import {UpdateTaskRequest} from '../../models/robots/tasks/requests/update-task-request';

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
    formData.append('categoryId', data.categoryId?.toString() ?? '');
    formData.append('priority', data.priority.toString());
    formData.append('complexity', data.complexity.toString());
    formData.append('requirements', JSON.stringify(data.requirements));
    formData.append('data', JSON.stringify(data.data));

    if (data.files) {
      data.files.forEach(file => {
        formData.append('files', file, file.name);
      });
    }

    return this.base.post<FormData, TaskBase>(url, formData);
  }

  reEnqueue(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}/enqueue`;
    return this.base.post<null, Success>(url, null);
  }

  updateTask(id:Guid, data: UpdateTaskRequest): Observable<TaskBase> {
    const url = `${this.baseUrl}/${id}`;

    const formData = new FormData();
    if (data.name) {
      formData.append('name', data.name);
    }

    if (data.description) {
      formData.append('description', data.description);
    }

    if (data.estimatedDuration) {
      formData.append('estimatedDuration', data.estimatedDuration);
    }

    if (data.priority) {
      formData.append('priority', data.priority.toString());
    }

    if (data.complexity) {
      formData.append('complexity', data.complexity.toString());
    }

    if (data.requirements) {
      formData.append('requirements', JSON.stringify(data.requirements));
    }

    if (data.data) {
      formData.append('data', JSON.stringify(data.data));
    }

    if (data.categoryId) {
      formData.append('categoryId', data.categoryId.toString());
    }

    if (data.files) {
      data.files.forEach(file => {
        formData.append('files', file, file.name);
      });
    }

    if (data.deletedRequirements && data.deletedRequirements.length > 0) {
      formData.append('deletedRequirements', JSON.stringify({
        ids: data.deletedRequirements,
      }));
    }

    if (data.deletedData && data.deletedData.length > 0) {
      formData.append('deletedData', JSON.stringify({
        ids: data.deletedData,
      }));
    }

    if (data.deletedFiles && data.deletedFiles.length > 0) {
      formData.append('deletedFiles', JSON.stringify({
        names: data.deletedFiles,
      }));
    }

    return this.base.put<FormData, TaskBase>(url, formData);
  }

  cancelTask(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}/status`;
    return this.base.delete<Success>(url);
  }

  deleteTask(id: Guid): Observable<Success> {
    const url = `${this.baseUrl}/${id}`;
    return this.base.delete<Success>(url);
  }
}
