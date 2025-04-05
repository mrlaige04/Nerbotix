import {Component, DestroyRef, inject, signal} from '@angular/core';
import {Button} from 'primeng/button';
import {Router, RouterLink} from '@angular/router';
import {InputText} from 'primeng/inputtext';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../../services/auth/auth.service';
import {catchError, finalize, of, takeUntil, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {HttpErrorResponse} from '@angular/common/http';
import {Message} from 'primeng/message';
import {BaseComponent} from '../../common/base/base.component';
import {FloatLabel} from 'primeng/floatlabel';

@Component({
  selector: 'rb-login',
  imports: [
    Button,
    RouterLink,
    InputText,
    ReactiveFormsModule,
    Message,
    FloatLabel
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);

  form = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    password: this.fb.control('', [Validators.required])
  });

  errorMessage = signal<string | null>(null);

  submit() {
    if (!this.form.valid) {
      return;
    }

    this.form.disable();
    this.showLoader();
    this.authService.login({
      email: this.form.value.email!,
      password: this.form.value.password!,
    }).pipe(
      catchError((error: HttpErrorResponse) => {
        this.errorMessage.set(error.error.detail);
        return of(null);
      }),
      tap(async (response) => {
        if (response) {
          this.authService.handleSuccessLogin(response);
          await this.router.navigate(['/']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe()
  }
}
