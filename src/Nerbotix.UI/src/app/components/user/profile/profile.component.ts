import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../common/base/base.component';
import {Image} from 'primeng/image';
import {Button} from 'primeng/button';
import {FloatLabel} from 'primeng/floatlabel';
import {InputText} from 'primeng/inputtext';
import {FileUpload} from 'primeng/fileupload';
import {RoleBase} from '../../../models/tenants/roles/role-base';
import {CurrentUser} from '../../../models/user/current-user';
import {catchError, finalize, of, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';

@Component({
  selector: 'nb-profile',
  imports: [
    Image,
    Button,
    FloatLabel,
    InputText,
    FileUpload
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent extends BaseComponent implements OnInit {
  private destroyRef = inject(DestroyRef);
  currentUser = signal<CurrentUser | null>(null);

  ngOnInit() {
    this.showLoader();
    this.currentUserService.getCurrentUser().pipe(
      catchError(_ => of(null)),
      tap(async (user) => {
        if (!user) {
          await this.router.navigate(['/']);
          return;
        }

        this.currentUser.set(user);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }

  createRolesString(roles: RoleBase[] | undefined) {
    if (!roles || !roles.length) {
      return 'None';
    }

    const roleNames = roles.map(r => r.name);
    return roleNames.reduce((r1, r2) => `${r1}, ${r2}`);
  }
}
