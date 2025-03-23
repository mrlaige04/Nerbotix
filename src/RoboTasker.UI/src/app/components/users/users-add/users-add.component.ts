import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
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
import {RolesService} from '../../../services/tenants/roles.service';
import {RoleBase} from '../../../models/tenants/roles/role-base';
import {MultiSelect} from 'primeng/multiselect';

@Component({
  selector: 'rb-users-add',
  imports: [
    ReactiveFormsModule,
    InputText,
    Button,
    MultiSelect
  ],
  templateUrl: './users-add.component.html',
  styleUrl: './users-add.component.scss'
})
export class UsersAddComponent extends BaseComponent implements OnInit {
  private usersService = inject(UsersService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private dialogRef = inject(DynamicDialogRef<UsersAddComponent>);
  private rolesService = inject(RolesService);

  form = this.fb.group({
    email: this.fb.control('', [Validators.required]),
    username: this.fb.control(null),
    roles: this.fb.control<RoleBase[]>([], [Validators.required])
  });

  roles = signal<RoleBase[]>([]);

  ngOnInit() {
    this.loadRoles();
  }

  private loadRoles() {
    this.showLoader();
    this.rolesService.getRoles({
      pageNumber: 1,
      pageSize: 99999
    }).pipe(
      tap((roles) => {
        this.roles.set(roles.items);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.hideLoader();
      })
    ).subscribe();
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    const formValue = this.form.value;
    const roles = formValue.roles?.map(r => r.id) ?? [];

    this.showLoader();
    const request: CreateUserRequest = {
      email: this.form.value.email!,
      username: this.form.value.username!,
      roles
    };
    this.usersService.createUser(request).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while creating user.`, errorMessage);
        return of(null);
      }),
      tap((res) => {
        if (res) {
          this.notificationService.showSuccess('OK', `User created`);
          this.dialogRef.close(true);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
