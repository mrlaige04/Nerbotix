import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {Card} from 'primeng/card';
import {NgxEchartsDirective} from 'ngx-echarts';
import {AnalyticsService} from '../../../services/analytics/analytics.service';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {tap} from 'rxjs';
import {BaseComponent} from '../../common/base/base.component';
import {BaseAnalyticComponent} from '../base-analytic/base-analytic.component';
import {TaskStatus} from '../../../models/robots/tasks/task-status';
import {EnumHelper} from '../../../utils/helpers/enum-helper';

@Component({
  selector: 'nb-tasks-statuses-analytic',
  imports: [
    Card,
    NgxEchartsDirective,
    BaseAnalyticComponent
  ],
  templateUrl: './tasks-statuses-analytic.component.html',
  styleUrl: './tasks-statuses-analytic.component.scss'
})
export class TasksStatusesAnalyticComponent extends BaseComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private analyticsService = inject(AnalyticsService);

  isLoading = signal<boolean>(false);

  private statusColorMap: Record<TaskStatus, string> = {
    [TaskStatus.Pending]: "#ffed91",
    [TaskStatus.WaitingForEnqueue]: "#8948fc",
    [TaskStatus.InProgress]: "#8d9be0",
    [TaskStatus.Completed]: "#5ced70",
    [TaskStatus.Failed]: "#eb4034",
    [TaskStatus.Canceled]: "#4d4a4a"
  };

  options: any = {
    tooltip: {
      trigger: 'item'
    },
    legend: {
      top: '5%',
      left: 'center'
    }
  };

  ngOnInit() {
    this.isLoading.set(true);
    this.analyticsService.getTaskStatuses().pipe(
      tap(result => {
        this.options.series = [{
          name: 'Task Statuses',
          type: 'pie',
          radius: ['30%', '70%'],
          avoidLabelOverlap: false,
          label: {
            show: false,
            position: 'center'
          },
          labelLine: {
            show: false
          },
          data: result.items.map(i => ({
            name: i.status,
            value: i.count,
            itemStyle: {
              color: this.statusColorMap[EnumHelper.getValueByName(TaskStatus, i.status)],
            }
          }))
        }];

        this.changeDetectorRef.detectChanges();

        this.isLoading.set(false);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }
}
