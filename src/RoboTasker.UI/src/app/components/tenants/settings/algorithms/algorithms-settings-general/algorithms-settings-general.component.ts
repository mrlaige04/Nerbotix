import {Component, DestroyRef, inject, input, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../../common/base/base.component';
import {AlgorithmCategory, AlgorithmName} from '../../../../../models/tenants/settings/algorithm-name';
import {SelectItem, SelectItemGroup} from 'primeng/api';
import {TenantSettingsService} from '../../../../../services/tenants/tenant-settings.service';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {TenantAlgorithmSettings} from '../../../../../models/tenants/settings/tenant-algorithms-settings';
import {Button} from 'primeng/button';
import {Select} from 'primeng/select';
import {FormsModule} from '@angular/forms';

@Component({
  selector: 'rb-algorithms-settings-general',
  imports: [
    Button,
    Select,
    FormsModule
  ],
  templateUrl: './algorithms-settings-general.component.html',
  styleUrl: './algorithms-settings-general.component.scss'
})
export class AlgorithmsSettingsGeneralComponent extends BaseComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  private settingsService = inject(TenantSettingsService);

  availableAlgorithms: AlgorithmName[] = AlgorithmName.All;
  groupedAlgorithms: SelectItemGroup[] = this.groupAlgorithmsByType(this.availableAlgorithms);

  settings = input<TenantAlgorithmSettings>();

  selectedAlgorithm = signal<AlgorithmName | null>(null);

  private findAlgorithmByName(name?: string): AlgorithmName {
    const result = this.availableAlgorithms.find(a => a.name === name);
    return result!;
  }

  private groupAlgorithmsByType(algorithms: AlgorithmName[]): SelectItemGroup[] {
    const grouped = algorithms.reduce((acc, algorithm) => {
      const type = AlgorithmCategory[algorithm.type];

      if (!acc[type]) {
        acc[type] = [];
      }

      acc[type].push({
        label: algorithm.displayName ?? algorithm.name,
        value: algorithm
      });

      return acc;
    }, {} as Record<string, SelectItem[]>);

    return Object.entries(grouped).map(([label, items]) => ({
      label,
      items
    }));
  }

  ngOnInit() {
    this.selectedAlgorithm.set(this.findAlgorithmByName(this.settings()?.preferredAlgorithm));
  }

  submit() {
    if (!this.selectedAlgorithm()) {
      return;
    }

    this.showLoader();
    this.settingsService.updateGeneralAlgorithmSettings({
      name: this.selectedAlgorithm()!.name
    }).pipe(
      catchError((error: HttpErrorResponse) => {
        const detail = error.error.detail;
        this.notificationService.showError('Error while updating settings', detail);
        return of(null);
      }),
      tap((res) => {
        if (res) {
          this.notificationService.showSuccess('OK', 'Settings updated successfully.');
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
