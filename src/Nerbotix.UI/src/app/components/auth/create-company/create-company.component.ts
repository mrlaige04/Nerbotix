import {Component, DestroyRef, inject, signal} from '@angular/core';
import {Button} from 'primeng/button';
import {FloatLabel} from 'primeng/floatlabel';
import {InputText} from 'primeng/inputtext';
import {BaseComponent} from '../../common/base/base.component';
import {AuthService} from '../../../services/auth/auth.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {Message} from 'primeng/message';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'nb-create-company',
  imports: [
    Button,
    FloatLabel,
    InputText,
    Message,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './create-company.component.html',
  styleUrl: './create-company.component.scss'
})
export class CreateCompanyComponent extends BaseComponent {
  private authService = inject(AuthService);
  private destroyRef = inject(DestroyRef);
  private fb = inject(FormBuilder);

  step = signal<CreateCompanyState>(CreateCompanyState.FIELDS);

  errorMessage = signal<string | null>(null);

  form = this.fb.group({
    email: this.fb.control('', [Validators.required, Validators.email]),
    name: this.fb.control('', [Validators.required])
  });

  submit() {
    this.showLoader();
    this.errorMessage.set(null);
    this.authService.createCompany({
      email: this.form.value.email!,
      name: this.form.value.name!
    }).pipe(
      catchError((err: HttpErrorResponse) => {
        const detail = err.error.detail;
        this.errorMessage.set(detail);
        return of(null);
      }),
      tap(res => {
        if (res) {
          this.step.set(CreateCompanyState.SUCCESS);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }

  protected readonly CreateCompanyState = CreateCompanyState;
}

export enum CreateCompanyState {
  FIELDS,
  SUCCESS
}
