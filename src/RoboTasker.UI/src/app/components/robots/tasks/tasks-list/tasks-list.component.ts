import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseTableListComponent} from '../../../common/base-table-list/base-table-list.component';
import {Guid} from 'guid-typescript';
import {finalize, Observable, tap} from 'rxjs';
import {Success} from '../../../../models/success';
import {TasksService} from '../../../../services/robots/tasks.service';
import {TableComponent} from '../../../common/table/table.component';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {TaskStatus} from '../../../../models/robots/tasks/task-status';
import {Button} from 'primeng/button';
import {Tooltip} from 'primeng/tooltip';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'rb-tasks-list',
  imports: [
    TableComponent,
    Button,
    Tooltip,
    RouterLink
  ],
  templateUrl: './tasks-list.component.html',
  styleUrl: './tasks-list.component.scss'
})
export class TasksListComponent extends BaseTableListComponent<any> implements OnInit {
  private tasksService = inject(TasksService);
  override destroyRef = inject(DestroyRef);

  constructor() {
    super();
    this.columns = [
      { label: 'Name', propName: 'name' },
      { label: 'Description', propName: 'description' },
      { label: 'Status', propName: 'status' },
      { label: 'Completed', propName: 'completedAt' },
      { label: 'Estimate', propName: 'estimatedDuration' },
      { label: 'Priority', propName: 'priority', sortable: true },
      { label: 'Complexity', propName: 'complexity' },
      { label: 'Robot', propName: 'assignedRobotId' }
    ];
  }

  ngOnInit() {
    this.getData();
  }

  openAddNew() {
    this.router.navigate(['tasks', 'add']);
  }

  openEditTask(id: Guid) {
    this.router.navigate(['tasks', id]);
  }

  override getData() {
    this.isLoading.set(true);
    this.tasksService.getTasks({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    }).pipe(
      tap(result => {
        this.items.set(result);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.isLoading.set(false);
      })
    ).subscribe();
  }

  override deleteItem(id: Guid): Observable<Success> {
    return this.tasksService.deleteTask(id);
  }

  protected readonly TaskStatus = TaskStatus;
}
