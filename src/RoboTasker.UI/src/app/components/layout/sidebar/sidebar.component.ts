import {Component, inject} from '@angular/core';
import {LayoutService} from '../../../services/layout/layout.service';
import {Divider} from 'primeng/divider';
import {Button} from 'primeng/button';
import {AuthService} from '../../../services/auth/auth.service';
import {BaseComponent} from '../../common/base/base.component';
import {MenuItem} from 'primeng/api';
import {Menu} from 'primeng/menu';
import {Ripple} from 'primeng/ripple';
import {RouterLink, RouterLinkActive} from '@angular/router';

@Component({
  selector: 'rb-sidebar',
  imports: [
    Divider,
    Button,
    Menu,
    Ripple,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent extends BaseComponent {
  private layoutService = inject(LayoutService);
  private authService = inject(AuthService);

  public sidebarOpened = this.layoutService.sidebarOpened;
  public isDesktop = this.layoutService.isDesktop;

  menu: MenuItem[] = [
    {
      label: 'Robots',
      items: [
        {
          label: 'Categories',
          routerLink: 'categories',
          icon: 'pi pi-bars'
        },
        {
          label: 'Robots',
          routerLink: 'robots',
          icon: 'pi pi-android'
        }
      ]
    },
    {
      label: 'Users Management',
      items: [
        {
          label: 'Users',
          routerLink: 'users',
          icon: 'pi pi-users'
        },
        {
          label: 'Roles',
          routerLink: 'roles',
          icon: 'pi pi-crown'
        }
      ]
    },
    {
      label: 'Settings',
      items: [
        {
          label: 'Settings',
          routerLink: 'user/settings',
          icon: 'pi pi-cog'
        }
      ]
    }
  ];

  closeSidebar() {
    this.layoutService.closeSidebar();
  }

  onNavigate(link: string) {
    if (!this.layoutService.isDesktop) {
      this.layoutService.closeSidebar();
    }

    this.router.navigateByUrl(link);
  }

  logout() {
    this.authService.logout();
  }
}
