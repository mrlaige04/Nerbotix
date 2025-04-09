import {Component, DestroyRef, inject, signal} from '@angular/core';
import {Button} from "primeng/button";
import {InputText} from "primeng/inputtext";
import {Message} from "primeng/message";
import {FormBuilder, ReactiveFormsModule, Validators} from "@angular/forms";
import {RouterLink} from "@angular/router";
import {AuthService} from '../../../services/auth/auth.service';
import {catchError, finalize, of, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {BaseComponent} from '../../common/base/base.component';
import {HttpErrorResponse} from '@angular/common/http';
import {InputOtp} from 'primeng/inputotp';
import {CustomValidators} from '../../../utils/validators/custom-validators';
import {ResetPasswordRequest} from '../../../models/auth/reset-password-request';
import {FloatLabel} from 'primeng/floatlabel';

@Component({
  selector: 'nb-forgot',
  imports: [
    Button,
    InputText,
    Message,
    ReactiveFormsModule,
    RouterLink,
    InputOtp,
    FloatLabel,
  ],
  templateUrl: './forgot.component.html',
  styleUrl: './forgot.component.scss'
})
export class ForgotComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);

  passwordRules = [
    { key: 'uppercase', message: 'The password must contain at least one uppercase letter (A-Z).' },
    { key: 'lowercase', message: 'The password must contain at least one lowercase letter (a-z).' },
    { key: 'digit', message: 'The password must contain at least one digit (0-9).' }
  ];

  form = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    code: this.fb.control('', [Validators.required]),
    password: this.fb.control('', [
      Validators.required, CustomValidators.Uppercase(),
      CustomValidators.Lowercase(), CustomValidators.Digit()]),
    confirmPassword: this.fb.control('', [
      Validators.required, CustomValidators.Compare('password')
    ])
  });

  emailControl = this.form.get('email');
  codeControl = this.form.get('code');
  passwordControl = this.form.get('password');
  confirmPasswordControl = this.form.get('confirmPassword');

  errorMessage = signal<string | null>(null);
  state = signal<ForgotRecoverState>(ForgotRecoverState.ENTER_EMAIL);

  next() {
    if (this.state() === ForgotRecoverState.ENTER_EMAIL) {
      this.sendEmail();
    } else if (this.state() === ForgotRecoverState.ENTER_CODE && this.codeControl?.valid) {
      this.state.set(ForgotRecoverState.ENTER_NEW_PASSWORD);
    } else {
      return;
    }
  }

  private sendEmail() {
    if (!this.emailControl?.valid) {
      return;
    }

    const email = this.emailControl.value!;
    this.form.disable();
    this.showLoader();
    this.errorMessage.set(null);
    this.authService.forgotPassword({ email }).pipe(
      catchError((error: HttpErrorResponse) => {
        this.errorMessage.set(error.error.detail);
        return of(null);
      }),
      tap((res) => {
        console.log(res)
        if (res) {
          this.state.set(ForgotRecoverState.ENTER_CODE);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    const request = this.form.value as ResetPasswordRequest;
    this.form.disable();
    this.showLoader();
    this.authService.resetPassword(request).pipe(
      catchError((error: HttpErrorResponse) => {
        this.errorMessage.set(error.error.detail);
        return of(null);
      }),
      tap(async (res) => {
        if (res) {
          await this.router.navigate(['auth', 'login']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  protected readonly ForgotRecoverState = ForgotRecoverState;
}

enum ForgotRecoverState {
  ENTER_EMAIL,
  ENTER_CODE,
  ENTER_NEW_PASSWORD,
  SUCCESS,
  FAILED
}
