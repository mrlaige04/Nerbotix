import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {NotificationService} from '../../services/common/notification.service';
import {catchError, throwError} from 'rxjs';
import {Router} from '@angular/router';

export const handleServerNotRespondingInterceptor: HttpInterceptorFn = (req, next) => {
  const notificationService = inject(NotificationService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 0 && !err.url?.endsWith('ping')) {
        notificationService.showError('SERVER ERROR', 'Currently server is not responding...');
        router.navigate(['maintenance']);
      }

      return throwError(() => err);
    })
  );
};
