import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {Card} from 'primeng/card';
import {BaseComponent} from '../../common/base/base.component';
import {AnalyticsService} from '../../../services/analytics/analytics.service';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {tap} from 'rxjs';
import {TaskBase} from '../../../models/robots/tasks/task-base';
import {TableColumn, TableComponent} from '../../common/table/table.component';

@Component({
  selector: 'nb-active-tasks-analytic',
  imports: [
    Card,
    TableComponent
  ],
  templateUrl: './active-tasks-analytic.component.html',
  styleUrl: './active-tasks-analytic.component.scss'
})
export class ActiveTasksAnalyticComponent extends BaseComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private analyticsService = inject(AnalyticsService);

  isLoading = signal<boolean>(false);
  tasks = signal<TaskBase[]>([]);

  columns: TableColumn[] = [
    { label: 'Name', propName: 'name' },
    { label: 'Priority', propName: 'priority' },
  ];

  private tasksCount = 7;

  ngOnInit() {
    this.analyticsService.getActiveTasks({
      pageNumber: 1,
      pageSize: this.tasksCount,
    }).pipe(
      tap(tasks => {
        this.tasks.set(tasks.items);
      }),
      takeUntilDestroyed(this.destroyRef),
    ).subscribe();
  }
}
