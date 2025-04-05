import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {TenantsService} from '../../../../services/super-admin/tenants.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {CreateTenantRequest} from '../../../../models/super-admin/tenants/requests/create-tenant-request';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Button} from 'primeng/button';
import {InputText} from 'primeng/inputtext';

@Component({
  selector: 'rb-tenant-add',
  imports: [
    ReactiveFormsModule,
    Button,
    InputText
  ],
  templateUrl: './tenant-add.component.html',
  styleUrl: './tenant-add.component.scss'
})
export class TenantAddComponent extends BaseComponent {
  private tenantsService = inject(TenantsService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private dialogRef = inject(DynamicDialogRef<TenantAddComponent>);

  form = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    name: this.fb.control(null, [Validators.required])
  });

  submit() {
    if (!this.form.valid) {
      return;
    }

    const request: CreateTenantRequest = {
      email: this.form.value.email!,
      name: this.form.value.name!
    };
    this.showLoader();
    this.tenantsService.createTenant(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while creating tenant`, errorMessage);
        return of(null);
      }),
      tap((res) => {
        if (res) {
          this.notificationService.showSuccess('OK', `Tenant created`);
          this.dialogRef.close(true);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
