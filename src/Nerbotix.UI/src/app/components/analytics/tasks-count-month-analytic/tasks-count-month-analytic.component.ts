import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {Card} from 'primeng/card';
import {BaseComponent} from '../../common/base/base.component';
import {AnalyticsService} from '../../../services/analytics/analytics.service';
import {BaseAnalyticComponent} from '../base-analytic/base-analytic.component';
import {tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

@Component({
  selector: 'nb-tasks-count-month-analytic',
  imports: [
    Card,
    BaseAnalyticComponent
  ],
  templateUrl: './tasks-count-month-analytic.component.html',
  styleUrl: './tasks-count-month-analytic.component.scss'
})
export class TasksCountMonthAnalyticComponent extends BaseComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private analyticsService = inject(AnalyticsService);

  isLoading = signal<boolean>(false);

  options: any = {
    xAxis: {
      type: 'category',
    },
    yAxis: {
      type: 'value',
    }
  };

  ngOnInit() {
    this.isLoading.set(true);
    this.analyticsService.getMonthTasksCreatedCount().pipe(
      tap(result => {
        this.options.xAxis.data = result.items.map(i => i.day);
        this.options.series = [{
          data: result.items.map(i => i.count),
          type: 'bar'
        }];

        this.changeDetectorRef.detectChanges();
        this.isLoading.set(false);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }
}
