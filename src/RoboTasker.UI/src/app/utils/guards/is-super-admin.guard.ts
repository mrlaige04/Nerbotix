import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {AuthService} from '../../services/auth/auth.service';

export const isSuperAdminGuard: CanActivateFn = async (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isSuperAdmin()) {
    await router.navigate(['no-access']);
    return false;
  }

  return true;
};
