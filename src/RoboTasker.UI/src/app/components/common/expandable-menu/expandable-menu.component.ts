import {Component, inject, input, output, signal} from '@angular/core';
import {RbMenuItem} from '../../layout/sidebar/sidebar.component';
import {Menu} from 'primeng/menu';
import {Button} from 'primeng/button';
import {HasPermissionDirective} from '../../../utils/directives/has-permission.directive';
import {Router, RouterLink, RouterLinkActive} from '@angular/router';
import {LayoutService} from '../../../services/layout/layout.service';
import {NgTemplateOutlet} from '@angular/common';

@Component({
  selector: 'rb-expandable-menu',
  imports: [
    Menu,
    Button,
    HasPermissionDirective,
    RouterLinkActive,
    RouterLink,
    NgTemplateOutlet
  ],
  templateUrl: './expandable-menu.component.html',
  styleUrl: './expandable-menu.component.scss'
})
export class ExpandableMenuComponent {
  private router = inject(Router);
  private layoutService = inject(LayoutService);
  menus = input.required<RbMenuItem[]>();

  activeMenu = input<RbMenuItem | null>();
  onNavigated = output<RbMenuItem>();

  isActive(item: RbMenuItem): boolean {
    return item.routerLink && this.activeMenu()?.routerLink === item.routerLink;
  }

  onNavigate(item: RbMenuItem) {
    if (!item.routerLink || !item.routerLink.length) {
      return;
    }

    if (this.activeMenu()?.routerLink === item.routerLink) {
      return;
    }

    this.onNavigated.emit(item);

    if (!this.layoutService.isDesktop) {
      this.layoutService.closeSidebar();
    }

    this.router.navigateByUrl(item.routerLink);
  }
}
