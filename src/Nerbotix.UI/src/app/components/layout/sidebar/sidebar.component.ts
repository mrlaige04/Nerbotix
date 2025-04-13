import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {LayoutService} from '../../../services/layout/layout.service';
import {Divider} from 'primeng/divider';
import {Button} from 'primeng/button';
import {AuthService} from '../../../services/auth/auth.service';
import {BaseComponent} from '../../common/base/base.component';
import {MenuItem} from 'primeng/api';
import {Ripple} from 'primeng/ripple';
import {NavigationEnd, RouterLink, RouterLinkActive} from '@angular/router';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';
import {HasPermissionDirective} from '../../../utils/directives/has-permission.directive';
import {JsonPipe} from '@angular/common';
import {CurrentUserService} from '../../../services/user/current-user.service';
import {RoleNames} from '../../../models/tenants/roles/roles-names';
import {HasRoleDirective} from '../../../utils/directives/has-role.directive';
import {ExpandableMenuComponent} from '../../common/expandable-menu/expandable-menu.component';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {filter, tap} from 'rxjs';

@Component({
  selector: 'nb-sidebar',
  imports: [
    Divider,
    Button,
    Ripple,
    RouterLink,
    RouterLinkActive,
    HasPermissionDirective,
    JsonPipe,
    HasRoleDirective,
    ExpandableMenuComponent
  ],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.scss'
})
export class SidebarComponent extends BaseComponent implements OnInit {
  private layoutService = inject(LayoutService);
  private authService = inject(AuthService);
  private currentUser = inject(CurrentUserService);
  private destroyRef = inject(DestroyRef);

  public sidebarOpened = this.layoutService.sidebarOpened;
  public isSuperAdmin = this.authService.isSuperAdmin;

  constructor() {
    super();
  }

  menu: RbMenuItem[] = [
    {
      label: 'Home',
      items: [
        {
          label: 'Dashboard',
          routerLink: 'dashboard',
          icon: 'pi pi-chart-bar',
        }
      ]
    },
    {
      label: 'Robots',
      items: [
        {
          label: 'Categories',
          routerLink: 'categories',
          icon: 'pi pi-bars',
          permission: PermissionsNames.CategoriesRead
        },
        {
          label: 'Capabilities',
          routerLink: 'capabilities',
          icon: 'pi pi-bolt',
          permission: PermissionsNames.CapabilitiesRead
        },
        {
          label: 'Robots',
          routerLink: 'robots',
          icon: 'pi pi-android',
          permission: PermissionsNames.RobotsRead
        },
        {
          label: 'Tasks',
          routerLink: 'tasks',
          icon: 'pi pi-check-square',
          permission: PermissionsNames.TasksRead
        }
      ]
    },
    {
      label: 'Users Management',
      items: [
        {
          label: 'Users',
          routerLink: 'tenant/users',
          icon: 'pi pi-users',
          permission: PermissionsNames.UsersRead
        },
        {
          label: 'Roles',
          routerLink: 'tenant/roles',
          icon: 'pi pi-crown',
          permission: PermissionsNames.RolesRead
        },
        {
          label: 'Permissions',
          routerLink: 'tenant/permissions',
          icon: 'pi pi-key',
          permission: PermissionsNames.PermissionsRead
        }
      ]
    },
    {
      label: 'Others',
      items: [
        {
          label: 'Settings',
          icon: 'pi pi-cog',
          permission: PermissionsNames.TenantSettingsRead,
          items: [
            {
              label: 'Algorithms',
              routerLink: 'tenant/settings/algorithms',
              icon: 'pi pi-microchip-ai',
              permission: PermissionsNames.TenantSettingsRead
            }
          ]
        }
      ]
    },
  ];
  superAdminMenu: RbMenuItem[] = [
    {
      label: 'Tenant Management',
      items: [
        {
          label: 'Tenants',
          routerLink: 'sa/tenants',
          icon: 'pi pi-users',
          role: RoleNames.SuperAdmin,
        },
        {
          label: 'Dashboard',
          routerLink: 'dashboard',
          icon: 'pi pi-chart-bar',
        }
      ]
    }
  ];
  filteredMenu: RbMenuItem[] = [];

  activeMenu = signal<RbMenuItem | null>(null);

  ngOnInit() {
    this.filteredMenu = this.filterMenuByPermissions(this.menu);
    this.setInitialActiveMenu();

    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        tap(() => this.setInitialActiveMenu()),
        takeUntilDestroyed(this.destroyRef)
      ).subscribe();
  }

  private setInitialActiveMenu() {
    const currentUrl = this.router.url;
    const path = this.findMenuPath(this.authService.isSuperAdmin()
      ? this.superAdminMenu
      : this.filteredMenu, currentUrl);

    if (path.length) {
      this.activeMenu.set(path[path.length - 1]);
      path.forEach(i => i.expanded = true);
    } else {
      this.activeMenu.set(null);
    }
  }

  private findMenuPath(menu: RbMenuItem[], currentUrl: string): RbMenuItem[] {
    for (const item of menu) {
      const itemLink = item.routerLink ?? '';

      if (itemLink && currentUrl.includes(itemLink)) {
        return [item];
      }

      if (item.items?.length) {
        const childPath = this.findMenuPath(item.items, currentUrl);
        if (childPath.length) {
          return [item, ...childPath];
        }
      }
    }

    return [];
  }

  onNavigate(item: RbMenuItem) {
    this.activeMenu.set(item);
  }

  filterMenuByPermissions(menu: RbMenuItem[]): RbMenuItem[] {
    return menu
      .map(category => ({
        ...category,
        items: category.items?.filter(item => this.hasPermission(item.permission)) ?? []
      }))
      .filter(category => category.items.length > 0);
  }

  override hasPermission(permission?: string) {
    if (!permission) return true;
    const user = this.currentUser.currentUser();
    return user?.permissions.some(p => p.name === permission) ?? false;
  }

  logout() {
    this.authService.logout();
  }
}

export interface RbMenuItem extends MenuItem {
  permission?: string;
  items?: RbMenuItem[];
}
