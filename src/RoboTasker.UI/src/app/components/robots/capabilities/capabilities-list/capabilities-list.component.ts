import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseTableListComponent} from '../../../common/base-table-list/base-table-list.component';
import {CapabilityBase} from '../../../../models/robots/capabilities/capability-base';
import {CapabilitiesService} from '../../../../services/robots/capabilities.service';
import {finalize, Observable, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Guid} from 'guid-typescript';
import {Success} from '../../../../models/success';
import {RobotStatus} from '../../../../models/robots/robots/robot-status';
import {Button} from 'primeng/button';
import {TableComponent} from '../../../common/table/table.component';

@Component({
  selector: 'rb-capabilities-list',
  imports: [
    Button,
    TableComponent
  ],
  templateUrl: './capabilities-list.component.html',
  styleUrl: './capabilities-list.component.scss'
})
export class CapabilitiesListComponent extends BaseTableListComponent<CapabilityBase> implements OnInit {
  private capabilitiesService = inject(CapabilitiesService);
  override destroyRef = inject(DestroyRef);

  constructor() {
    super();

    this.columns = [
      { label: 'Group name', propName: 'groupName' },
      { label: 'Description', propName: 'description' },
      { label: 'Capabilities count', propName: 'capabilitiesCount' },
    ];
  }

  ngOnInit() {
    this.getData();
  }

  openAddNew() {
    this.router.navigate(['capabilities', 'add']);
  }

  override getData() {
    this.isLoading.set(true);
    this.capabilitiesService.getCapabilities({
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

  openEditCapability(id: Guid) {
    this.router.navigate(['capabilities', id]);
  }

  override deleteItem(id: Guid): Observable<Success> {
    return this.capabilitiesService.deleteCapability(id);
  }

  protected readonly RobotStatus = RobotStatus;
}
