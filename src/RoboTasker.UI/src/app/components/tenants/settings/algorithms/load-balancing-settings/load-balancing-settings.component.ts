import {Component, DestroyRef, inject, input, OnInit} from '@angular/core';
import {LoadBalancingAlgorithmSettings} from '../../../../../models/tenants/settings/tenant-algorithms-settings';
import {BaseComponent} from '../../../../common/base/base.component';
import {TenantSettingsService} from '../../../../../services/tenants/tenant-settings.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Button} from 'primeng/button';
import {InputNumber} from 'primeng/inputnumber';

@Component({
  selector: 'rb-load-balancing-settings',
  imports: [
    ReactiveFormsModule,
    Button,
    InputNumber
  ],
  templateUrl: './load-balancing-settings.component.html',
  styleUrl: './load-balancing-settings.component.scss'
})
export class LoadBalancingSettingsComponent extends BaseComponent implements OnInit{
  private settingsService = inject(TenantSettingsService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  settings = input<LoadBalancingAlgorithmSettings>();

  form = this.fb.group({
    complexityFactor: this.fb.control(this.settings()?.complexityFactor, Validators.required),
  });

  ngOnInit() {
    this.form.patchValue({
      complexityFactor: this.settings()?.complexityFactor,
    });
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    this.showLoader();
    const request: LoadBalancingAlgorithmSettings = {
      complexityFactor: this.form.value.complexityFactor!
    };
    this.settingsService.updateLoadBalancingAlgorithm(request).pipe(
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
