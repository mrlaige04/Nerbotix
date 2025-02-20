import {Component, inject} from '@angular/core';
import {Button} from 'primeng/button';
import {LayoutService} from '../../../services/layout/layout.service';

@Component({
  selector: 'rb-topbar',
  imports: [
    Button
  ],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss'
})
export class TopbarComponent {
  private layoutService = inject(LayoutService);

  toggleSidebar() {
    this.layoutService.toggleSidebar();
  }
}
