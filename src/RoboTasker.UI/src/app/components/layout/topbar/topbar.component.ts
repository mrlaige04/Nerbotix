import {Component, computed, inject} from '@angular/core';
import {Button} from "primeng/button";
import {LayoutService} from '../../../services/layout/layout.service';
import {Avatar} from 'primeng/avatar';
import {Menu} from 'primeng/menu';
import {MenuItem} from 'primeng/api';
import {AuthService} from '../../../services/auth/auth.service';
import {BaseComponent} from '../../common/base/base.component';
import {BadgeDirective} from 'primeng/badge';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'rb-topbar',
  imports: [
    Button,
    Avatar,
    Menu,
    BadgeDirective,
    RouterLink
  ],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss'
})
export class TopbarComponent extends BaseComponent {
  private layoutService = inject(LayoutService);
  private authService = inject(AuthService);

  toggleSidebar(event: Event) {
    event.stopPropagation();
    this.layoutService.toggleSidebar();
  }
}
