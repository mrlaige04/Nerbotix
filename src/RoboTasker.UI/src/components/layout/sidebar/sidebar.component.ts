import {Component, inject} from '@angular/core';
import {LayoutService} from "../../../services/layout/layout.service";

@Component({
  selector: 'rb-sidebar',
  imports: [],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent {
  private layoutService = inject(LayoutService);
  public sidebarOpened = this.layoutService.sidebarOpened;


}
