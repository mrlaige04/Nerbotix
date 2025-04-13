import {Component, DestroyRef, inject, OnInit} from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import {GlobalLoaderComponent} from './components/common/global-loader/global-loader.component';
import {Toast} from 'primeng/toast';
import {ConfirmPopup} from 'primeng/confirmpopup';
import {ConfirmDialog} from 'primeng/confirmdialog';
import {AuthService} from './services/auth/auth.service';
import {CurrentUserService} from './services/user/current-user.service';
import {catchError, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, GlobalLoaderComponent, Toast, ConfirmPopup, ConfirmDialog],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  private authService = inject(AuthService);
  private currentUserService = inject(CurrentUserService);
  private destroyRef = inject(DestroyRef);
  private router = inject(Router);

  title = 'RoboTasker';

  ngOnInit() {
    if (this.authService.isAuthenticated()) {
      this.currentUserService.getCurrentUser().pipe(
        catchError((error: HttpErrorResponse) => {
          this.router.navigate(['auth']);
          return of(null);
        }),
        tap(user => {
          if (user) {
            this.currentUserService.setCurrentUser(user);
          }
        }),
        takeUntilDestroyed(this.destroyRef)
      ).subscribe();
    }
  }
}
