import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {BaseHttp} from '../../../services/base/base-http';
import {catchError, of} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {HttpErrorResponse} from '@angular/common/http';

@Component({
  selector: 'nb-maintenance',
  imports: [],
  templateUrl: './maintenance.component.html',
  styleUrl: './maintenance.component.scss',
})
export class MaintenanceComponent extends BaseComponent implements OnInit {
  private http = inject(BaseHttp);
  private destroyRef = inject(DestroyRef);

  ngOnInit() {
    this.http.get('ping').pipe(
      catchError(async (error: HttpErrorResponse) => {
        if (error.status !== 0) {
          await this.router.navigate(['/']);
        }

        return of(null);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe()
  }
}
