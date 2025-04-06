import { Component } from '@angular/core';
import {RouterLink, RouterLinkActive, RouterOutlet} from '@angular/router';
import {BaseComponent} from '../../common/base/base.component';
import {
  LoadBalancingSettingsComponent
} from '../settings/algorithms/load-balancing-settings/load-balancing-settings.component';
import {
  LinearOptimizationSettingsComponent
} from '../settings/algorithms/linear-optimization-settings/linear-optimization-settings.component';

@Component({
  selector: 'rb-tenant-settings-wrapper',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    LoadBalancingSettingsComponent,
    LinearOptimizationSettingsComponent
  ],
  templateUrl: './tenant-settings-wrapper.component.html',
  styleUrl: './tenant-settings-wrapper.component.scss'
})
export class TenantSettingsWrapperComponent extends BaseComponent {
}

