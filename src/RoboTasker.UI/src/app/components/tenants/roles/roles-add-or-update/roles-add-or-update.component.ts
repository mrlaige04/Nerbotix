import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {RolesService} from '../../../../services/tenants/roles.service';
import {FormArray, FormBuilder, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {Role} from '../../../../models/tenants/role';
import {Guid} from 'guid-typescript';
import {PermissionsService} from '../../../../services/tenants/permissions.service';
import {catchError, finalize, forkJoin, of, switchMap, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {CreateRoleRequest} from '../../../../models/tenants/roles/requests/create-role-request';
import {UpdateRoleRequest} from '../../../../models/tenants/roles/requests/update-role-request';
import {PermissionGroup} from '../../../../models/tenants/permissions/permission-group';
import {Button} from 'primeng/button';
import {Divider} from 'primeng/divider';
import {InputText} from 'primeng/inputtext';
import {ToggleSwitch, ToggleSwitchChangeEvent} from 'primeng/toggleswitch';

@Component({
  selector: 'rb-roles-add-or-update',
  imports: [
    ReactiveFormsModule,
    Button,
    Divider,
    InputText,
    ToggleSwitch,
    FormsModule
  ],
  templateUrl: './roles-add-or-update.component.html',
  styleUrl: './roles-add-or-update.component.scss'
})
export class RolesAddOrUpdateComponent extends BaseComponent implements OnInit {
  private rolesService = inject(RolesService);
  private permissionsService = inject(PermissionsService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);

  isEdit = signal<boolean>(false);

  currentRole = signal<Role | null>(null);
  currentRoleId = signal<Guid | null>(null);

  selectedPermissions = signal<Guid[]>([]);
  deletedPermissions = signal<Guid[]>([]);

  form = this.fb.group({
    name: this.fb.control('', [Validators.required]),
    permissions: this.fb.array([])
  });

  permissions = signal<PermissionGroup[]>([]);

  permissionsArray = this.form.get('permissions') as FormArray;

  ngOnInit() {
    const isEdit = this.detectViewMode();
    if (isEdit) {
      this.loadData();
    } else {
      this.loadPermissions();
    }
  }

  private loadData() {
    forkJoin([this.getPermissionsSource(), this.loadExistingRoleSource()])
      .pipe(
        tap(([permissions, role]) => {
          this.permissions.set(permissions);
          if (role) {
            this.currentRole.set(role);
            this.initializeFormFromRole(role);
          }
        })
      ).subscribe();
  }

  private getPermissionsSource() {
    return this.permissionsService.getGroups({
      pageNumber: 1,
      pageSize: 99999
    }).pipe(
      switchMap((list) => {
        return forkJoin(list.items.map(i => this.permissionsService.getGroup(i.id)))
      }),
      tap((result) => {
        this.permissions.set(result);
      })
    );
  }

  private loadPermissions() {
    this.getPermissionsSource().subscribe();
  }

  private detectViewMode() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.currentRoleId.set(id);
      this.isEdit.set(true);
      return true;
    }

    return false;
  }

  private loadExistingRoleSource() {
    return this.rolesService.getRole(this.currentRoleId()!)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorMessage = error.error?.detail;
          this.notificationService.showError('Role not found', errorMessage);
          this.currentRoleId.set(null);
          this.isEdit.set(false);
          return of(null);
        }),
        tap((role) => {
          if (role) {
            this.isEdit.set(true);
            this.currentRole.set(role);
            this.initializeFormFromRole(role);
          }
        }),
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.hideLoader();
        })
      );
  }

  onPermissionToggleChange(id: Guid, event: ToggleSwitchChangeEvent) {
    if (event.checked) {
      this.deletedPermissions.update(d => {
        return d.filter(p => p !== id);
      });
      this.selectedPermissions.update(s => {
        return s.some(p => p === id) ? s : [...s, id];
      })
    } else {
      this.selectedPermissions.update(d => {
        return d.filter(p => p !== id);
      });
      this.deletedPermissions.update(d => {
        return d.some(p => p === id) ? d : [...d, id];
      })
    }
  }

  ifPermissionSelected(id: Guid) {
    return this.selectedPermissions().some(p => p === id);
  }

  private initializeFormFromRole(role: Role) {
    this.form.patchValue({ name: role.name });

    const selectedPermission = role.permissions.map(p => p.id);
    this.selectedPermissions.set(selectedPermission);
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    const isEdit = this.isEdit() && !!this.currentRoleId;
    const action = isEdit ? 'Updating' : 'Adding';
    const observable = isEdit ?
      this.updateRole() : this.createRole();

    this.form.disable();
    this.showLoader();
    observable.pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while ${action.toLowerCase()} role`, errorMessage);
        return of(null);
      }),
      tap(async (result) => {
        if (result) {
          this.notificationService.showSuccess('OK', `Role was ${isEdit ? 'updated': 'added'}.`);
          await this.router.navigate(['tenant', 'roles']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  private createRole() {
    const request: CreateRoleRequest = {
      name: this.form.value.name!,
      permissions: this.selectedPermissions()
    };

    return this.rolesService.createRole(request);
  }

  private updateRole() {
    const permissions = this.selectedPermissions().filter(
      p => !this.currentRole()?.permissions.some(rp => rp.id === p)
    );

    const request: UpdateRoleRequest = {
      name: this.form.value.name!,
      permissions,
      deletePermissions: this.deletedPermissions()
    };

    return this.rolesService.updateRole(this.currentRoleId()!, request);
  }

  protected readonly FormArray = FormArray;
}
