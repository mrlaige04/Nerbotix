import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {AnalyticsService} from '../../../services/analytics/analytics.service';
import {tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Card} from 'primeng/card';
import {BaseAnalyticComponent} from '../base-analytic/base-analytic.component';

@Component({
  selector: 'nb-robots-statuses-analytic',
  imports: [
    Card,
    BaseAnalyticComponent
  ],
  templateUrl: './robots-statuses-analytic.component.html',
  styleUrl: './robots-statuses-analytic.component.scss'
})
export class RobotsStatusesAnalyticComponent extends BaseComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private analyticsService = inject(AnalyticsService);

  isLoading = signal<boolean>(false);

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
    this.analyticsService.getRobotStatuses().pipe(
      tap(result => {
        this.options.series = [{
          name: 'Robot Statuses',
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
            value: i.count
          }))
        }];

        this.changeDetectorRef.detectChanges();
        this.isLoading.set(false);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }
}
