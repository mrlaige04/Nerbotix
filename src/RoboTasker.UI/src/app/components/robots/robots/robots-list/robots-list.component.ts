import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseTableListComponent} from '../../../common/base-table-list/base-table-list.component';
import {RobotBase} from '../../../../models/robots/robots/robot-base';
import {RobotsService} from '../../../../services/robots/robots.service';
import { finalize, Observable, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Guid} from 'guid-typescript';
import {Success} from '../../../../models/success';
import {TableComponent} from '../../../common/table/table.component';
import {TableModule} from 'primeng/table';
import {PaginatorModule} from 'primeng/paginator';
import {Toolbar} from 'primeng/toolbar';
import {Button} from 'primeng/button';
import {DatePipe, NgTemplateOutlet} from '@angular/common';
import {ProgressBar} from 'primeng/progressbar';
import {FormsModule} from '@angular/forms';
import {RobotStatus} from '../../../../models/robots/robots/robot-status';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'rb-robots-list',
  imports: [
    TableModule,
    PaginatorModule,
    Toolbar,
    Button,
    NgTemplateOutlet,
    ProgressBar,
    TableComponent,
    FormsModule,
    RouterLink,
    DatePipe,
  ],
  templateUrl: './robots-list.component.html',
  styleUrl: './robots-list.component.scss',
})
export class RobotsListComponent extends BaseTableListComponent<RobotBase> implements OnInit {
  private robotsService = inject(RobotsService);
  override destroyRef = inject(DestroyRef);

  constructor() {
    super();
    this.columns = [
      { label: 'Name', propName: 'name' },
      { label: 'Category', propName: (robot) => robot['category']['name'] },
      { label: 'Status', propName: 'status' },
      { label: 'Location', propName: 'location' },
      { label: 'Location timestamp', propName: '' },
      { label: 'Last activity', propName: 'lastActivity' }
    ];
  }

  ngOnInit() {
    this.getData();
  }

  openAddNew() {
    this.router.navigate(['robots', 'add'])
  }

  override getData() {
    this.isLoading.set(true);
    this.robotsService.getRobots({
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

  openEditRobot(id: Guid) {
    this.router.navigate(['robots', id]);
  }

  override deleteItem(id: Guid): Observable<Success> {
    return this.robotsService.deleteRobot(id);
  }

  protected readonly RobotStatus = RobotStatus;
}
