import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {UIChart} from 'primeng/chart';
import {Divider} from 'primeng/divider';
import {LayoutService} from '../../services/layout/layout.service';
import {NgxEchartsDirective} from 'ngx-echarts';

@Component({
  selector: 'rb-home',
  imports: [
    UIChart,
    Divider,
    NgxEchartsDirective
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit, OnDestroy {
  private layoutService = inject(LayoutService);

  data: any;
  options: any;

  ngOnInit() {
    this.layoutService.wrapToCard.set(false);
    this.options = {
      xAxis: {
        type: 'category',
        data: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
      },
      yAxis: {
        type: 'value'
      },
      series: [{
        data: [150, 230, 224, 218, 154, 25, 235],
        type: 'bar',
      }]
    }
  }

  ngOnDestroy() {
    this.layoutService.wrapToCard.set(true);
  }
}
