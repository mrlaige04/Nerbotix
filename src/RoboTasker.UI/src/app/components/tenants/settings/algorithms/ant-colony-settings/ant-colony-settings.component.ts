import {Component, DestroyRef, inject, input, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../common/base/base.component';
import {TenantSettingsService} from '../../../../../services/tenants/tenant-settings.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {
  AntColonyAlgorithmSettings
} from '../../../../../models/tenants/settings/tenant-algorithms-settings';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Button} from 'primeng/button';
import {InputNumber} from 'primeng/inputnumber';

@Component({
  selector: 'rb-ant-colony-settings',
  imports: [
    Button,
    InputNumber,
    ReactiveFormsModule
  ],
  templateUrl: './ant-colony-settings.component.html',
  styleUrl: './ant-colony-settings.component.scss'
})
export class AntColonySettingsComponent extends BaseComponent implements OnInit{
  private settingsService = inject(TenantSettingsService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  settings = input<AntColonyAlgorithmSettings>();

  form = this.fb.group({
    antCount: this.fb.control(this.settings()?.antCount, Validators.required),
    iterations: this.fb.control(this.settings()?.iterations, Validators.required),
    evaporation: this.fb.control(this.settings()?.evaporation, Validators.required),
    alpha: this.fb.control(this.settings()?.alpha, Validators.required),
    beta: this.fb.control(this.settings()?.beta, Validators.required),
  });

  ngOnInit() {
    this.form.patchValue({
      antCount: this.settings()?.antCount,
      iterations: this.settings()?.iterations,
      evaporation: this.settings()?.evaporation,
      alpha: this.settings()?.alpha,
      beta: this.settings()?.beta,
    });
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    this.showLoader();
    const request: AntColonyAlgorithmSettings = {
      antCount: this.form.value.antCount!,
      iterations: this.form.value.iterations!,
      evaporation: this.form.value.evaporation!,
      alpha: this.form.value.alpha!,
      beta: this.form.value.beta!,
    };
    this.settingsService.updateAntColonyAlgorithm(request).pipe(
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
