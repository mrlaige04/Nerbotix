import { HttpInterceptorFn } from '@angular/common/http';
import {inject} from '@angular/core';
import {AuthService} from '../../services/auth/auth.service';

export const passTokenInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  let headers = req.headers;

  if (authService.isAuthenticated() && authService.accessToken()) {
    const token = authService.accessToken()!;
    headers = headers.set('Authorization', `${token.tokenType} ` + token.accessToken);
  }

  return next(req.clone({
    headers: headers
  }));
};
