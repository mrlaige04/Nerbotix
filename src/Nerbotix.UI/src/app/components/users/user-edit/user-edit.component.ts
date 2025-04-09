import {Component, computed, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {UsersService} from '../../../services/users/users.service';
import {Guid} from 'guid-typescript';
import {User} from '../../../models/users/user';
import {catchError, finalize, of, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {RolesService} from '../../../services/tenants/roles.service';
import {HttpErrorResponse} from '@angular/common/http';
import {FormBuilder, FormControl, ReactiveFormsModule, Validators} from '@angular/forms';
import {Divider} from 'primeng/divider';
import {InputText} from 'primeng/inputtext';
import {Button} from 'primeng/button';
import {RoleBase} from '../../../models/tenants/roles/role-base';
import {MultiSelect} from 'primeng/multiselect';
import {UpdateUserRequest} from '../../../models/users/requests/update-user-request';

@Component({
  selector: 'nb-user-edit',
  imports: [
    ReactiveFormsModule,
    Divider,
    InputText,
    Button,
    MultiSelect
  ],
  templateUrl: './user-edit.component.html',
  styleUrl: './user-edit.component.scss'
})
export class UserEditComponent extends BaseComponent implements OnInit {
  private usersService = inject(UsersService);
  private destroyRef = inject(DestroyRef);
  private rolesService = inject(RolesService);
  private fb = inject(FormBuilder);

  form = this.fb.group({
    email: new FormControl({ value: '', disabled: true }),
    username: this.fb.control('', [Validators.required]),
    roles: this.fb.control<RoleBase[] | null>([], [Validators.required]),
  });

  currentUserId = signal<Guid | null>(null);
  currentUser = signal<User | null>(null);

  roles = signal<RoleBase[]>([]);

  ngOnInit() {
    this.detectUser();
    this.loadRoles();
  }

  private loadRoles() {
    this.showLoader();
    this.rolesService.getRoles({
      pageNumber: 1,
      pageSize: 9999
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

  private detectUser() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (!id) {
      this.router.navigate(['tenant', 'users']);
      return;
    }

    this.currentUserId.set(id);
    this.loadUser();
  }

  private loadUser() {
    this.showLoader();
    this.usersService.getUser(this.currentUserId()!).pipe(
      catchError((error: HttpErrorResponse) => {
        return of(null);
      }),
      tap((user) => {
        if (!user) {
          this.router.navigate(['tenant', 'users']);
          return;
        }

        this.currentUser.set(user);
        this.initializeFormFromUser(user);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }

  private initializeFormFromUser(user: User) {
    this.form.patchValue(user);
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    const formValue = this.form.value;

    const roles = formValue.roles?.map(r => r.id) ?? [];
    const initialRoles = this.currentUser()?.roles.map(r => r.id) ?? [];
    const newRoles = roles?.filter(r => !initialRoles.includes(r));
    const deletedRoles = initialRoles.filter(r => !roles.includes(r));

    const request: UpdateUserRequest = {
      username: formValue.username!,
      roles: newRoles,
      deleteRoles: deletedRoles
    };

    this.showLoader();
    this.usersService.updateUser(this.currentUserId()!, request)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          return of(null)
        }),
        tap((res) => {
          if (res) {
            this.router.navigate(['tenant', 'users']);
          }
        }),
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.hideLoader();
        })
      ).subscribe();
  }
}
