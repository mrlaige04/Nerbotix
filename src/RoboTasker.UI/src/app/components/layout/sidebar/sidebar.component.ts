import {Component, inject} from '@angular/core';
import {LayoutService} from '../../../services/layout/layout.service';
import {Divider} from 'primeng/divider';
import {Button} from 'primeng/button';
import {AuthService} from '../../../services/auth/auth.service';
import {BaseComponent} from '../../common/base/base.component';

@Component({
  selector: 'rb-sidebar',
  imports: [
    Divider,
    Button
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent extends BaseComponent {
  private layoutService = inject(LayoutService);
  private authService = inject(AuthService);

  public sidebarOpened = this.layoutService.sidebarOpened;

  logout() {
    this.authService.logout();
  }
}
