import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {LoadBalancingSettingsComponent} from '../algorithms/load-balancing-settings/load-balancing-settings.component';
import {
  LinearOptimizationSettingsComponent
} from '../algorithms/linear-optimization-settings/linear-optimization-settings.component';
import {Divider} from 'primeng/divider';
import {Tab, TabList, TabPanel, TabPanels, Tabs} from 'primeng/tabs';
import {BaseComponent} from '../../../common/base/base.component';
import {TenantAlgorithmSettings} from '../../../../models/tenants/settings/tenant-algorithms-settings';
import {TenantSettingsService} from '../../../../services/tenants/tenant-settings.service';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {GeneticSettingsComponent} from '../algorithms/genetic-settings/genetic-settings.component';
import {AntColonySettingsComponent} from '../algorithms/ant-colony-settings/ant-colony-settings.component';
import {
  SimulatedAnnealingSettingsComponent
} from '../algorithms/simulated-annealing-settings/simulated-annealing-settings.component';
import {
  AlgorithmsSettingsGeneralComponent
} from '../algorithms/algorithms-settings-general/algorithms-settings-general.component';

@Component({
  selector: 'rb-algorithms-settings-wrapper',
  imports: [
    LoadBalancingSettingsComponent,
    LinearOptimizationSettingsComponent,
    Divider,
    Tabs,
    TabList,
    Tab,
    TabPanels,
    TabPanel,
    GeneticSettingsComponent,
    AntColonySettingsComponent,
    SimulatedAnnealingSettingsComponent,
    AlgorithmsSettingsGeneralComponent
  ],
  templateUrl: './algorithms-settings-wrapper.component.html',
  styleUrl: './algorithms-settings-wrapper.component.scss'
})
export class AlgorithmsSettingsWrapperComponent extends BaseComponent implements OnInit {
  private settingsService = inject(TenantSettingsService);
  private destroyRef = inject(DestroyRef);

  settings = signal<TenantAlgorithmSettings | null>(null);

  ngOnInit() {
    this.loadSettings();
  }

  private loadSettings() {
    this.showLoader();
    this.settingsService.getAlgorithmSettings().pipe(
      catchError((error: HttpErrorResponse) => {
        const detail = error.error.detail;
        this.notificationService.showError('Error while getting settings', detail);
        return of(null);
      }),
      tap((settings) => {
        if (settings) {
          this.settings.set(settings);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
