import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {AnalyticsService} from '../../../services/analytics/analytics.service';
import {tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Card} from 'primeng/card';
import {BaseAnalyticComponent} from '../base-analytic/base-analytic.component';

@Component({
  selector: 'nb-robot-count-by-categories-analytic',
  imports: [
    Card,
    BaseAnalyticComponent
  ],
  templateUrl: './robot-count-by-categories-analytic.component.html',
  styleUrl: './robot-count-by-categories-analytic.component.scss'
})
export class RobotCountByCategoriesAnalyticComponent extends BaseComponent implements OnInit {
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
    this.analyticsService.getRobotCountByCategories().pipe(
      tap(result => {
        this.options.series = [{
          name: 'Robots by Categories',
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
            name: i.categoryName,
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
