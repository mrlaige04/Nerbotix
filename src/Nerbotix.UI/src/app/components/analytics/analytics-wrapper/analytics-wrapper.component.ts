import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {ActiveTasksAnalyticComponent} from "../active-tasks-analytic/active-tasks-analytic.component";
import {RobotsStatusesAnalyticComponent} from "../robots-statuses-analytic/robots-statuses-analytic.component";
import {TasksCountMonthAnalyticComponent} from "../tasks-count-month-analytic/tasks-count-month-analytic.component";
import {TasksStatusesAnalyticComponent} from "../tasks-statuses-analytic/tasks-statuses-analytic.component";
import {BaseComponent} from '../../common/base/base.component';
import {LayoutService} from '../../../services/layout/layout.service';
import {AuthService} from '../../../services/auth/auth.service';
import {
  RobotCountByCategoriesAnalyticComponent
} from '../robot-count-by-categories-analytic/robot-count-by-categories-analytic.component';

@Component({
  selector: 'nb-analytics-wrapper',
  imports: [
    ActiveTasksAnalyticComponent,
    RobotsStatusesAnalyticComponent,
    TasksCountMonthAnalyticComponent,
    TasksStatusesAnalyticComponent,
    RobotCountByCategoriesAnalyticComponent
  ],
  templateUrl: './analytics-wrapper.component.html',
  styleUrl: './analytics-wrapper.component.scss'
})
export class AnalyticsWrapperComponent extends BaseComponent implements OnInit, OnDestroy {
  private authService = inject(AuthService);
  private layoutService = inject(LayoutService);

  isSuperAdmin = this.authService.isSuperAdmin;

  ngOnInit() {
    this.layoutService.wrapToCard.set(false);
  }

  ngOnDestroy() {
    this.layoutService.wrapToCard.set(true);
  }
}
