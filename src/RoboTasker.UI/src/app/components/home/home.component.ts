import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {UIChart} from 'primeng/chart';
import {Divider} from 'primeng/divider';
import {LayoutService} from '../../services/layout/layout.service';

@Component({
  selector: 'rb-home',
  imports: [
    UIChart,
    Divider
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
    this.data = {
      labels: ['Q1','Q2','Q3','Q4'],
      datasets: [
        {
          label: 'Sales',
          data: [400, 200, 150, 400]
        }
      ]
    };
  }

  ngOnDestroy() {
    this.layoutService.wrapToCard.set(true);
  }
}
