import {Component, computed, inject} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {Toolbar} from 'primeng/toolbar';
import {RouterLink} from '@angular/router';
import {UiSettingsService} from '../../../services/layout/ui-settings.service';
import {LayoutService} from '../../../services/layout/layout.service';
import {AuthService} from '../../../services/auth/auth.service';
import {Avatar} from 'primeng/avatar';
import {Button} from 'primeng/button';
import {Menu} from 'primeng/menu';
import {MenuItem} from 'primeng/api';

@Component({
  selector: 'rb-navbar',
  imports: [
    Toolbar,
    RouterLink,
    Avatar,
    Button,
    Menu
  ],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss'
})
export class NavbarComponent extends BaseComponent {
  private uiSettingsService = inject(UiSettingsService);
  private layoutService = inject(LayoutService);
  private authService = inject(AuthService);

  theme = this.uiSettingsService.theme;

  private readonly authenticatedUserMenu: MenuItem[] = [
    { label: 'Profile', icon: 'pi pi-user', routerLink: '/user/profile' },
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

  toggleSidebar(event: Event) {
    event.stopPropagation();
    this.layoutService.toggleSidebar();
  }
}
