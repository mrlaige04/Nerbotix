import {Component, computed, inject} from '@angular/core';
import {Button} from "primeng/button";
import {LayoutService} from '../../../services/layout/layout.service';
import {Avatar} from 'primeng/avatar';
import {Menu} from 'primeng/menu';
import {MenuItem, MenuItemCommandEvent} from 'primeng/api';
import {AuthService} from '../../../services/auth/auth.service';
import {BaseComponent} from '../../common/base/base.component';

@Component({
  selector: 'rb-topbar',
  imports: [
    Button,
    Avatar,
    Menu
  ],
  templateUrl: './topbar.component.html',
  styleUrl: './topbar.component.scss'
})
export class TopbarComponent extends BaseComponent {
  private layoutService = inject(LayoutService);
  private authService = inject(AuthService);

  private readonly authenticatedUserMenu: MenuItem[] = [
    { label: 'Profile', icon: 'pi pi-user', routerLink: '/user/profile' },
    { label: 'Settings', icon: 'pi pi-cog', routerLink: '/user/settings' },
    { label: 'Logout', icon: 'pi pi-sign-out', routerLink: '/auth/logout' },
  ];
  private readonly notAuthenticatedUserMenu: MenuItem[] = [
    { label: 'Login', icon: 'pi pi-sign-in', routerLink: '/auth/login' },
  ];

  userMenuItems = computed(() => {
    if (this.authService.isAuthenticated()) {
      return this.authenticatedUserMenu;
    }

    return this.notAuthenticatedUserMenu;
  });

  toggleSidebar() {
    this.layoutService.toggleSidebar();
  }
}
