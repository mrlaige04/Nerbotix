import {Component, DestroyRef, inject} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {UsersService} from '../../../services/users/users.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {CreateUserRequest} from '../../../models/users/requests/create-user-request';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {InputText} from 'primeng/inputtext';
import {Button} from 'primeng/button';

@Component({
  selector: 'rb-users-add',
  imports: [
    ReactiveFormsModule,
    InputText,
    Button
  ],
  templateUrl: './users-add.component.html',
  styleUrl: './users-add.component.scss'
})
export class UsersAddComponent extends BaseComponent {
  private usersService = inject(UsersService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private dialogRef = inject(DynamicDialogRef<UsersAddComponent>);

  form = this.fb.group({
    email: this.fb.control('', [Validators.required]),
    username: this.fb.control(null),
  });

  submit() {
    if (!this.form.valid) {
      return;
    }

    this.showLoader();
    const request = this.form.value as CreateUserRequest;
    this.usersService.createUser(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while creating user.`, errorMessage);
        return of(null);
      }),
      tap(() => {
        this.notificationService.showSuccess('OK', `User created`);
        this.dialogRef.close(true);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
