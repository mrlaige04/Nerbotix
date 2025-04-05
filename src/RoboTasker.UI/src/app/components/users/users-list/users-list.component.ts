import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseTableListComponent} from '../../common/base-table-list/base-table-list.component';
import {UserBase} from '../../../models/users/user-base';
import {UsersService} from '../../../services/users/users.service';
import {finalize, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Guid} from 'guid-typescript';
import {TableComponent} from '../../common/table/table.component';
import {Button} from 'primeng/button';
import { DynamicDialogRef} from 'primeng/dynamicdialog';
import {UsersAddComponent} from '../users-add/users-add.component';
import {HasPermissionDirective} from '../../../utils/directives/has-permission.directive';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';

@Component({
  selector: 'rb-users-list',
  imports: [
    TableComponent,
    Button,
    HasPermissionDirective,
  ],
  templateUrl: './users-list.component.html',
  styleUrl: './users-list.component.scss'
})
export class UsersListComponent extends BaseTableListComponent<UserBase> implements OnInit {
  private usersService = inject(UsersService);
  override destroyRef = inject(DestroyRef);
  private dialogRef: DynamicDialogRef<UsersAddComponent> | undefined;

  currentUser = this.currentUserService.currentUser;

  constructor() {
    super();
    this.columns = [
      { label: 'Email', propName: 'email' },
      { label: 'Username', propName: 'username' },
      { label: 'Roles', propName: 'roles' },
      { label: 'Verified', propName: 'emailVerified' },
    ];
  }

  ngOnInit() {
    this.getData();
  }

  openAddNew() {
    this.dialogRef = this.dialogService.open(UsersAddComponent, {
      modal: true,
      header: 'Create user',
      styleClass: 'w-100',
      style: {
        minWidth: '40%'
      },
      closable: true,
      resizable: true
    });

    this.dialogRef.onClose.pipe(
      tap((result) => {
        if (result === true) {
          this.getData();
        }
      }),
      takeUntilDestroyed(this.destroyRef),
    ).subscribe();
  }

  openEditUser(id: Guid) {
    this.router.navigate(['tenant', 'users', id]);
  }

  override getData() {
    this.isLoading.set(true);
    this.usersService.getUsers({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    }).pipe(
      tap((result) => {
        this.items.set(result);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.isLoading.set(false);
      })
    ).subscribe();
  }

  override deleteItem(id: Guid) {
    return this.usersService.deleteUser(id);
  }

  protected readonly PermissionsNames = PermissionsNames;
}
