import {Component, DestroyRef, inject, input, OnInit} from '@angular/core';
import {BaseComponent} from '../../../../common/base/base.component';
import {TenantSettingsService} from '../../../../../services/tenants/tenant-settings.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {
  GeneticAlgorithmSettings
} from '../../../../../models/tenants/settings/tenant-algorithms-settings';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Button} from 'primeng/button';
import {InputNumber} from 'primeng/inputnumber';

@Component({
  selector: 'rb-genetic-settings',
  imports: [
    ReactiveFormsModule,
    Button,
    InputNumber
  ],
  templateUrl: './genetic-settings.component.html',
  styleUrl: './genetic-settings.component.scss'
})
export class GeneticSettingsComponent extends BaseComponent implements OnInit{
  private settingsService = inject(TenantSettingsService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  settings = input<GeneticAlgorithmSettings>();

  form = this.fb.group({
    populationSize: this.fb.control(this.settings()?.populationSize, Validators.required),
    generations: this.fb.control(this.settings()?.generations, Validators.required),
    mutationRate: this.fb.control(this.settings()?.mutationRate, Validators.required),
  });

  ngOnInit() {
    this.form.patchValue({
      populationSize: this.settings()?.populationSize,
      generations: this.settings()?.generations,
      mutationRate: this.settings()?.mutationRate
    });
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    this.showLoader();
    const request: GeneticAlgorithmSettings = {
      populationSize: this.form.value.populationSize!,
      mutationRate: this.form.value.mutationRate!,
      generations: this.form.value.generations!,
    };

    this.settingsService.updateGeneticAlgorithm(request).pipe(
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
