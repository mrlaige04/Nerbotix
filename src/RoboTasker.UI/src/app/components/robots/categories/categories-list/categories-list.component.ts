import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {CategoriesService} from '../../../../services/robots/categories.service';
import { finalize, Observable, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {TableModule} from 'primeng/table';
import {Toolbar} from 'primeng/toolbar';
import {Button} from 'primeng/button';
import {IconField} from 'primeng/iconfield';
import {InputIcon} from 'primeng/inputicon';
import {InputText} from 'primeng/inputtext';
import {RouterLink} from '@angular/router';
import {TableComponent} from '../../../common/table/table.component';
import {JsonPipe} from '@angular/common';
import {CategoryAddOrUpdateComponent} from '../category-add-or-update/category-add-or-update.component';
import {DialogService, DynamicDialogRef} from 'primeng/dynamicdialog';
import {CategoryBase} from '../../../../models/robots/categories/category-base';
import {BaseTableListComponent} from '../../../common/base-table-list/base-table-list.component';
import {Guid} from 'guid-typescript';
import {Success} from '../../../../models/success';
import {HasPermissionDirective} from '../../../../utils/directives/has-permission.directive';
import {PermissionsNames} from '../../../../models/tenants/permissions/permissions-names';

@Component({
  selector: 'rb-categories-list',
  imports: [
    TableModule,
    Toolbar,
    Button,
    IconField,
    InputIcon,
    InputText,
    RouterLink,
    TableComponent,
    JsonPipe,
    HasPermissionDirective
  ],
  templateUrl: './categories-list.component.html',
  styleUrl: './categories-list.component.scss',
  viewProviders: [DialogService]
})
export class CategoriesListComponent extends BaseTableListComponent<CategoryBase> implements OnInit {
  private categoriesService = inject(CategoriesService);
  override destroyRef = inject(DestroyRef);

  dialogRef: DynamicDialogRef<CategoryAddOrUpdateComponent> | undefined;

  constructor() {
    super();

    this.columns = [
      { label: 'Name', propName: 'name' }
    ];
  }

  ngOnInit() {
    this.getData();
  }

  openAddNew() {
    this.dialogRef = this.dialogService.open<CategoryAddOrUpdateComponent>(CategoryAddOrUpdateComponent, {
      modal: true,
      header: 'Create new category',
      closable: true,
      resizable: true
    });

    this.dialogRef.onClose.pipe(
      tap((result) => {
        if (result == true) {
          this.getData();
        }
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  openEditCategory(id: Guid) {
    this.dialogRef = this.dialogService.open<CategoryAddOrUpdateComponent>(CategoryAddOrUpdateComponent, {
      data: {
        isEdit: true,
        id
      },
      modal: true,
      header: 'Edit category',
      resizable: true,
      closable: true
    });
  }

  override getData() {
    this.isLoading.set(true);
    this.categoriesService.getCategories({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
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

  override deleteItem(id: Guid): Observable<Success> {
    return this.categoriesService.deleteCategory(id);
  }

  protected readonly PermissionsNames = PermissionsNames;
}
