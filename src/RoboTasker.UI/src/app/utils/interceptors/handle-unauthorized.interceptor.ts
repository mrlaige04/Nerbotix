import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {AuthService} from '../../services/auth/auth.service';
import {NotificationService} from '../../services/common/notification.service';
import {catchError, throwError} from 'rxjs';

export const handleUnauthorizedInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const notificationService = inject(NotificationService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401) {
        notificationService.showError('Unauthorized', 'Please login to the account.');
        authService.logout();
      }

      return throwError(() => err);
    })
  );
};
