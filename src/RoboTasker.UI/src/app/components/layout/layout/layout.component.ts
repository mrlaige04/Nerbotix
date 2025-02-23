import {Component, inject} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {SidebarComponent} from '../sidebar/sidebar.component';
import {TopbarComponent} from '../topbar/topbar.component';
import {LayoutService} from '../../../services/layout/layout.service';
import {BaseComponent} from '../../common/base/base.component';
import {Card} from 'primeng/card';

@Component({
  selector: 'rb-layout',
  imports: [
    RouterOutlet,
    SidebarComponent,
    TopbarComponent,
    Card
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent extends BaseComponent {
  private layoutService = inject(LayoutService);
  sidebarOpened = this.layoutService.sidebarOpened;
}
