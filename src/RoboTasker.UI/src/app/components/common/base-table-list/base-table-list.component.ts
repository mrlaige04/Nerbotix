import {Component, DestroyRef, output, signal} from '@angular/core';
import {BaseComponent} from '../base/base.component';
import {PageOptions, TableColumn} from '../table/table.component';
import {Guid} from 'guid-typescript';
import {catchError, finalize, Observable, of, tap} from 'rxjs';
import {IEntity} from '../../../models/common/entity';
import {PaginatedList} from '../../../models/common/paginated-list';
import {TableModule} from 'primeng/table';
import {FormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

@Component({
  selector: 'rb-base-table-list',
  imports: [
    TableModule,
    FormsModule,
    CommonModule
  ],
  templateUrl: './base-table-list.component.html',
  styleUrl: './base-table-list.component.scss'
})
export abstract class BaseTableListComponent<T extends IEntity<Guid>> extends BaseComponent {
  pageNumber = signal<number>(1);
  pageSize = signal<number>(10);

  isLoading = signal<boolean>(false);

  items = signal<PaginatedList<T>>({
    items: [],
    pageNumber: 0,
    pageSize: 0,
    totalItems: 0
  });

  protected abstract destroyRef: DestroyRef;

  columns: TableColumn[] = [];

  selectedItems = signal<T[]>([]);

  onPageChange(state: PageOptions) {
    this.pageNumber.set(state.pageNumber);
    this.pageSize.set(state.pageSize);

    this.getData();
  }

  onItemsSelect(items: T[]) {
    this.selectedItems.set(items);
  }

  openDeleteConfirmation(id: Guid, event: Event) {
    this.confirmationService.confirm({
      target: event.target as EventTarget,
      header: 'Confirmation',
      icon: 'pi pi-info-circle',
      rejectLabel: 'Cancel',
      rejectButtonProps: {
        label: 'Cancel',
        outlined: true,
        severity: 'secondary'
      },
      acceptButtonProps: {
        label: 'Delete',
        severity: 'danger'
      },
      message: 'Are you sure you want to delete this item?',
      accept: () => {
        this.deleteItem(id).pipe(
          catchError((err: HttpErrorResponse) => {
            const detail = err.error.detail;
            this.notificationService.showError('Error while deleting', detail);
            return of(null);
          }),
          tap((result) => {
            if (result) {
              this.getData();
            }
          }),
          takeUntilDestroyed(this.destroyRef),
          finalize(() => {
            this.isLoading.set(false);
          })
        ).subscribe();
      }
    });
  }

  protected abstract getData(): void;
  protected abstract deleteItem(id: Guid): Observable<any | void>;
}
