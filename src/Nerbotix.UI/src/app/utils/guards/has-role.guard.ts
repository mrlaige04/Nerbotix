import {CanActivateFn, Router} from '@angular/router';
import {inject} from '@angular/core';
import {CurrentUserService} from '../../services/user/current-user.service';

export const hasRoleGuard: CanActivateFn = async (route, state) => {
  const role = route.data['role'];
  if (!role) {
    return true;
  }

  const currentUserService = inject(CurrentUserService);
  const user = currentUserService.currentUser();
  const router  = inject(Router);

  if (!user) {
    await router.navigate(['no-access']);
    return false;
  }

  const hasRole = user.roles.some(p => p.name === role);
  if (!hasRole) {
    await router.navigate(['no-access']);
    return false;
  }

  return true;
};
