import {Component, inject, input} from '@angular/core';
import {RbMenuItem} from '../../layout/sidebar/sidebar.component';
import {Menu} from 'primeng/menu';
import {Button} from 'primeng/button';
import {HasPermissionDirective} from '../../../utils/directives/has-permission.directive';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {LayoutService} from '../../../services/layout/layout.service';

@Component({
  selector: 'rb-expandable-menu',
  imports: [
    Menu,
    Button,
    HasPermissionDirective,
    RouterLinkActive,
    RouterLink
  ],
  templateUrl: './expandable-menu.component.html',
  styleUrl: './expandable-menu.component.scss'
})
export class ExpandableMenuComponent {
  private router = inject(Router);
  private layoutService = inject(LayoutService);
  menus = input.required<RbMenuItem[]>();

  onNavigate(link: string) {
    if (!this.layoutService.isDesktop) {
      this.layoutService.closeSidebar();
    }

    this.router.navigateByUrl(link);
  }
}
