import {Component, DestroyRef, inject, signal} from '@angular/core';
import {Button} from 'primeng/button';
import {InputText} from 'primeng/inputtext';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {CustomValidators} from '../../../utils/validators/custom-validators';
import {BaseComponent} from '../../common/base/base.component';
import {AuthService} from '../../../services/auth/auth.service';
import {ChangePasswordRequest} from '../../../models/auth/change-password-request';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Message} from 'primeng/message';
import {FloatLabel} from 'primeng/floatlabel';

@Component({
  selector: 'nb-change-password',
  imports: [
    Button,
    InputText,
    ReactiveFormsModule,
    Message,
    FloatLabel
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.scss'
})
export class ChangePasswordComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);

  form = this.fb.group({
    currentPassword: this.fb.control('', [Validators.required]),
    newPassword: this.fb.control('', [
      Validators.required, CustomValidators.Uppercase(),
      CustomValidators.Lowercase(), CustomValidators.Digit()]),
    confirmPassword: this.fb.control('', [
      Validators.required, CustomValidators.Compare('newPassword')
    ])
  });

  errorMessage = signal<string | null>(null);

  submit() {
    if (!this.form.valid) {
      return;
    }

    const request = this.form.value as ChangePasswordRequest;
    this.form.disable();
    this.showLoader();
    this.errorMessage.set(null);
    this.authService.changePassword(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let message = '';
        const errors = error.error.errors;
        for (const key in errors) {
          message += `${errors[key]}\n`;
        }

        this.errorMessage.set(message);
        return of(null);
      }),
      tap(async (res) => {
        if (res) {
          this.notificationService.showSuccess('Password changed successfully');
          this.form.reset();
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }
}
