import {Component, inject} from '@angular/core';
import {SidebarComponent} from '../sidebar/sidebar.component';
import {TopbarComponent} from "../topbar/topbar.component";
import {RouterOutlet} from "@angular/router";
import {LayoutService} from "../../../services/layout/layout.service";

@Component({
  selector: 'rb-layout',
  imports: [
    SidebarComponent,
    TopbarComponent,
    RouterOutlet
  ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent {
  private layoutService = inject(LayoutService);
  sidebarOpened = this.layoutService.sidebarOpened;
}
