import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {CurrentUserService} from '../../services/user/current-user.service';

export const hasPermissionGuard: CanActivateFn = async (route, state) => {
  const permission = route.data['permission'];
  const currentUserService = inject(CurrentUserService);
  const user = currentUserService.currentUser();
  const router = inject(Router);

  if (!user) {
    await router.navigate(['/']);
    return false;
  }

  const hasPermission = user.permissions.some(p => p.name === permission);
  if (!hasPermission) {
    await router.navigate(['/']);
    return false;
  }

  return true;
};
