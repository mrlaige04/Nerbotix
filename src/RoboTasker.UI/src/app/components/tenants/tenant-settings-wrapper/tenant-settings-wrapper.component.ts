import { Component } from '@angular/core';
import {RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';
import {BaseComponent} from '../../common/base/base.component';

@Component({
  selector: 'rb-tenant-settings-wrapper',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './tenant-settings-wrapper.component.html',
  styleUrl: './tenant-settings-wrapper.component.scss'
})
export class TenantSettingsWrapperComponent extends BaseComponent {
  menus: TenantSettingsItem[] = [
    {
      label: 'Algorithms',
      icon: 'pi pi-forward',
      children: [
        {
          label: 'Linear Optimization',
          routerLink: 'algorithms/linear-optimization',
          icon: 'pi pi-angle-double-up',
        }
      ]
    }
  ];

  toggleExpansion(menu: TenantSettingsItem) {
    menu.expanded = !menu.expanded;
  }
}

interface TenantSettingsItem {
  label: string;
  icon?: string;
  routerLink?: string;
  children?: TenantSettingsItem[];
  expanded?: boolean;
}
