import {Component, DestroyRef, inject, input, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../common/base/base.component';
import {TenantSettingsService} from '../../../../../services/tenants/tenant-settings.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {
  LoadBalancingAlgorithmSettings,
  SimulatedAnnealingAlgorithmSettings
} from '../../../../../models/tenants/settings/tenant-algorithms-settings';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Button} from 'primeng/button';
import {InputNumber} from 'primeng/inputnumber';

@Component({
  selector: 'nb-simulated-annealing-settings',
  imports: [
    Button,
    InputNumber,
    ReactiveFormsModule
  ],
  templateUrl: './simulated-annealing-settings.component.html',
  styleUrl: './simulated-annealing-settings.component.scss'
})
export class SimulatedAnnealingSettingsComponent extends BaseComponent implements OnInit{
  private settingsService = inject(TenantSettingsService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  settings = input<SimulatedAnnealingAlgorithmSettings>();

  form = this.fb.group({
    initialTemperature: this.fb.control(this.settings()?.initialTemperature, Validators.required),
    coolingRate: this.fb.control(this.settings()?.coolingRate, Validators.required),
    iterationsPerTemp: this.fb.control(this.settings()?.iterationsPerTemp, Validators.required),
    minTemperature: this.fb.control(this.settings()?.minTemperature, Validators.required)
  });

  ngOnInit() {
    this.form.patchValue({
      initialTemperature: this.settings()?.initialTemperature,
      coolingRate: this.settings()?.coolingRate,
      iterationsPerTemp: this.settings()?.iterationsPerTemp,
      minTemperature: this.settings()?.minTemperature,
    });
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    this.showLoader();
    const request: SimulatedAnnealingAlgorithmSettings = {
      initialTemperature: this.form.value.initialTemperature!,
      coolingRate: this.form.value.coolingRate!,
      iterationsPerTemp: this.form.value.iterationsPerTemp!,
      minTemperature: this.form.value.minTemperature!
    };
    this.settingsService.updateSimulatedAnnealingAlgorithm(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const detail = error.error.detail;
        this.notificationService.showError('Error while updating settings', detail);
        return of(null);
      }),
      tap((success) => {
        if (success) {
          this.router.navigate(['tenant', 'settings', 'algorithms'])
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
