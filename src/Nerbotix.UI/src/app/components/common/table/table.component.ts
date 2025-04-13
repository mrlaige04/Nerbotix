import {
  AfterContentChecked,
  AfterViewInit,
  Component,
  computed,
  ContentChild,
  input,
  output,
  signal,
  TemplateRef, WritableSignal
} from '@angular/core';
import {TableModule} from 'primeng/table';
import {Paginator, PaginatorState} from 'primeng/paginator';
import {Toolbar} from 'primeng/toolbar';
import {Button} from 'primeng/button';
import {CommonModule, NgTemplateOutlet} from '@angular/common';
import {ProgressBar} from 'primeng/progressbar';
import {FormsModule} from '@angular/forms';
import {HasPermissionDirective} from '../../../utils/directives/has-permission.directive';
import {BaseComponent} from '../base/base.component';
import {PermissionsNames} from '../../../models/tenants/permissions/permissions-names';

@Component({
  selector: 'nb-table',
  imports: [CommonModule, TableModule, Paginator, Toolbar, Button, NgTemplateOutlet, ProgressBar, FormsModule, HasPermissionDirective],
  templateUrl: './table.component.html',
  styleUrl: './table.component.scss'
})
export class TableComponent extends BaseComponent implements AfterViewInit, AfterContentChecked {
  loading = input<boolean>(false);

  @ContentChild('caption') captionTemplateRef?: TemplateRef<any>;
  @ContentChild('header') headerTemplateRef?: TemplateRef<any>;
  @ContentChild('body') bodyTemplateRef?: TemplateRef<any>;
  @ContentChild('toolbarStart') toolbarStartTemplateRef?: TemplateRef<any>;
  @ContentChild('toolbarEnd') toolbarEndTemplateRef?: TemplateRef<any>;

  @ContentChild('rowActions') rowActionsTemplateRef?: TemplateRef<any>;

  captionTemplate = signal<TemplateRef<any> | undefined>(undefined);
  headerTemplate = signal<TemplateRef<any> | undefined>(undefined);
  bodyTemplate = signal<TemplateRef<any> | undefined>(undefined);
  toolbarStartTemplate = signal<TemplateRef<any> | undefined>(undefined);
  toolbarEndTemplate = signal<TemplateRef<any> | undefined>(undefined);

  rowActionTemplate = signal<TemplateRef<any> | undefined>(undefined);

  addPermission = input<string>();

  selectedItems = signal<any[]>([]);

  dataKey = input<string>('id');
  showRowActions = input<boolean>(false);


  ngAfterViewInit() {
    this.captionTemplate.set(this.captionTemplateRef);
    this.headerTemplate.set(this.headerTemplateRef);
    this.bodyTemplate.set(this.bodyTemplateRef);
    this.toolbarStartTemplate.set(this.toolbarStartTemplateRef);
    this.toolbarEndTemplate.set(this.toolbarEndTemplateRef);
    this.rowActionTemplate.set(this.rowActionsTemplateRef);
  }

  ngAfterContentChecked() {
    this.updateTemplateIfChanged(this.captionTemplate, this.captionTemplateRef);
    this.updateTemplateIfChanged(this.headerTemplate, this.headerTemplateRef);
    this.updateTemplateIfChanged(this.bodyTemplate, this.bodyTemplateRef);
    this.updateTemplateIfChanged(this.toolbarStartTemplate, this.toolbarStartTemplateRef);
    this.updateTemplateIfChanged(this.toolbarEndTemplate, this.toolbarEndTemplateRef);
    this.updateTemplateIfChanged(this.rowActionTemplate, this.rowActionsTemplateRef);
  }

  private updateTemplateIfChanged(signalVal: WritableSignal<TemplateRef<any> | undefined>,
                                  newValue: TemplateRef<any> | undefined) {
    if (signalVal() !== newValue) {
      signalVal.set(newValue);
    }
  }

  pageNumberInput = input<number>(1, { alias: 'pageNumber' });
  pageSizeInput = input<number>(10, { alias: 'pageSize' });
  totalItems = input.required<number>();
  pageSizeOptionsInput = input<number[]>([5, 10, 25, 50], { alias: 'pageSizeOptions' });

  pageNumber = signal<number>(this.pageNumberInput());
  pageSize = signal<number>(this.pageSizeInput());
  pageSizeOptions = signal<number[]>(this.pageSizeOptionsInput());

  hidePagination = input<boolean>(false);
  showGridlines = input<boolean>(true);

  rowsToSkip = computed(() => (this.pageNumber() - 1) * this.pageSize());

  data = input.required<any[]>();
  columns = input.required<TableColumn[]>();

  showToolbar = input<boolean>(false);

  onSelectionChange(data: any) {
    this.selectionChange.emit(data);
  }

  onPageChange(state: PaginatorState) {
    this.pageSize.set(state.rows!);
    this.pageNumber.set(state.page! + 1);

    this.pageChange.emit({
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize()
    })
  }

  onSort(event: any) {
    console.log(event)
    // TODO: implement sorting
  }

  onAddClick = output();
  selectionChange = output<any[]>();

  pageChange = output<PageOptions>();

  isPropNameFunction(prop: any) {
    return typeof prop === 'function';
  }

  protected readonly PermissionsNames = PermissionsNames;
}

export interface PageOptions {
  pageNumber: number;
  pageSize: number;
}

export interface TableColumn {
  label: string;
  propName: string | ((any: any) => any);
  sortable?: boolean;
}
