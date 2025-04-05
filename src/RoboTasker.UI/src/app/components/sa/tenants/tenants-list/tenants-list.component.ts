import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseTableListComponent} from '../../../common/base-table-list/base-table-list.component';
import {TenantBase} from '../../../../models/super-admin/tenants/tenant-base';
import {TenantsService} from '../../../../services/super-admin/tenants.service';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import { finalize, tap} from 'rxjs';
import {Guid} from 'guid-typescript';
import {TableComponent} from '../../../common/table/table.component';
import {PermissionsNames} from '../../../../models/tenants/permissions/permissions-names';
import {Button} from 'primeng/button';
import {HasPermissionDirective} from '../../../../utils/directives/has-permission.directive';
import {DynamicDialogRef} from 'primeng/dynamicdialog';
import {TenantAddComponent} from '../tenant-add/tenant-add.component';

@Component({
  selector: 'rb-tenants-list',
  imports: [
    TableComponent,
    Button,
    HasPermissionDirective
  ],
  templateUrl: './tenants-list.component.html',
  styleUrl: './tenants-list.component.scss'
})
export class TenantsListComponent extends BaseTableListComponent<TenantBase> implements OnInit {
  private tenantsService = inject(TenantsService);
  override destroyRef = inject(DestroyRef);
  private dialogRef: DynamicDialogRef<TenantAddComponent> | undefined;

  constructor() {
    super();
    this.columns = [
      { label: 'Email', propName: 'email' },
      { label: 'Name', propName: 'name' },
    ];
  }

  ngOnInit() {
    this.getData();
  }

  openAddNew() {
    this.dialogRef = this.dialogService.open(TenantAddComponent, {
      modal: true,
      header: 'Create tenant',
      styleClass: 'w-100',
      style: {
        minWidth: '30%'
      },
      closable: true,
      resizable: true
    });

    this.dialogRef.onClose.pipe(
      tap(result => {
        if (result === true) {
          this.getData();
        }
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  openEditTenant() {

  }

  override getData() {
    this.isLoading.set(true);
    this.tenantsService.getTenants({
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
    return this.tenantsService.deleteTenant(id);
  }

  protected readonly PermissionsNames = PermissionsNames;
}
