import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {AuthService} from '../../services/auth/auth.service';
import {NotificationService} from '../../services/common/notification.service';
import {catchError, switchMap, throwError} from 'rxjs';

export const handleUnauthorizedInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const notificationService = inject(NotificationService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401 && !err.url?.includes('auth')) {
        const token = authService.accessToken();
        if (!token || !token.refreshToken) {
          notificationService.showError('Unauthorized', 'Please login to the account.');
          authService.logout();
          return throwError(() => err);
        }

        return authService.refreshToken().pipe(
          switchMap((newToken) => {
            if (newToken && newToken.accessToken) {
              authService.updateToken(newToken);

              return next(req.clone({
                setHeaders: {
                  Authorization: `Bearer ${newToken.accessToken}`
                }
              }));
            } else {
              notificationService.showError('Unauthorized', 'Please login to the account.');
              authService.logout();
              return throwError(() => err);
            }
          }),
          catchError((refreshError) => {
            notificationService.showError('Unauthorized', 'Please login to the account.');
            authService.logout();
            return throwError(() => refreshError);
          })
        );
      }

      return throwError(() => err);
    })
  );
};
