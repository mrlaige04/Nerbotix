import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseTableListComponent} from '../../../common/base-table-list/base-table-list.component';
import {PermissionGroupBase} from '../../../../models/tenants/permissions/permission-group-base';
import {PermissionsService} from '../../../../services/tenants/permissions.service';
import {Guid} from 'guid-typescript';
import {finalize, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {TableComponent} from '../../../common/table/table.component';
import {Button} from 'primeng/button';

@Component({
  selector: 'rb-permissions-list',
  imports: [
    TableComponent,
    Button
  ],
  templateUrl: './permissions-list.component.html',
  styleUrl: './permissions-list.component.scss'
})
export class PermissionsListComponent extends BaseTableListComponent<PermissionGroupBase> implements OnInit {
  private permissionsService = inject(PermissionsService);
  override destroyRef = inject(DestroyRef);

  constructor() {
    super();
    this.columns = [
      { label: 'Name', propName: 'name' },
      { label: 'System', propName: 'isSystem' },
    ];
  }

  ngOnInit() {
    this.getData();
  }

  openAddNew() {
    this.router.navigate(['tenant', 'permissions', 'add']);
  }

  openEditPermission(id: Guid) {
    this.router.navigate(['tenant', 'permissions', id]);
  }

  override getData() {
    this.isLoading.set(true);
    this.permissionsService.getGroups({
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
    return this.permissionsService.deleteGroup(id);
  }
}
