import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../../services/auth/auth.service';
import {RegisterRequest} from '../../../models/auth/register-request';
import {catchError, finalize, of, takeUntil, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {CustomValidators} from '../../../utils/validators/custom-validators';
import {Button} from 'primeng/button';
import {InputText} from 'primeng/inputtext';
import {FloatLabel} from 'primeng/floatlabel';

@Component({
  selector: 'rb-register',
  imports: [
    ReactiveFormsModule,
    Button,
    InputText,
    FloatLabel
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);

  form = this.fb.group({
    email: this.fb.control('', [Validators.required]),
    token: this.fb.control('', [Validators.required]),
    password: this.fb.control('', [Validators.required]),
    confirmPassword: this.fb.control('', [Validators.required, CustomValidators.Compare('password')])
  });

  ngOnInit() {
    const email = this.activatedRoute.snapshot.queryParams['email'];
    const token = this.activatedRoute.snapshot.queryParams['token'];

    this.form.patchValue({ email, token });
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    this.showLoader();
    const request = this.form.value as RegisterRequest;
    this.authService.register(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while registration`, errorMessage);
        return of(null);
      }),
      tap(async () => {
        await this.router.navigate(['auth', 'login'])
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
