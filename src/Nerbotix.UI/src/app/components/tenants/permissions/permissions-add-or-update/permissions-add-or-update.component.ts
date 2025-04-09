import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {PermissionsService} from '../../../../services/tenants/permissions.service';
import {FormArray, FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {CreatePermissionRequest} from '../../../../models/tenants/permissions/requests/create-permission-request';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {PermissionGroup} from '../../../../models/tenants/permissions/permission-group';
import {Guid} from 'guid-typescript';
import {UpdatePermissionRequest} from '../../../../models/tenants/permissions/requests/update-permission-request';
import {InputText} from 'primeng/inputtext';
import {Divider} from 'primeng/divider';
import {Button} from 'primeng/button';

@Component({
  selector: 'nb-permissions-add-or-update',
  imports: [
    ReactiveFormsModule,
    InputText,
    Divider,
    Button
  ],
  templateUrl: './permissions-add-or-update.component.html',
  styleUrl: './permissions-add-or-update.component.scss'
})
export class PermissionsAddOrUpdateComponent extends BaseComponent implements OnInit {
  private permissionsService = inject(PermissionsService);
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);

  isEdit = signal<boolean>(false);

  currentGroup = signal<PermissionGroup | null>(null);
  currentGroupId = signal<Guid | null>(null);

  deletedPermissions = signal<Guid[]>([]);

  form = this.fb.group({
    name: this.fb.control('', [Validators.required]),
    permissions: this.fb.array([])
  });

  permissionsArray = this.form.get('permissions') as FormArray;

  addNewPermission() {
    const group = this.fb.group({
      name: this.fb.control('', [Validators.required]),
      isSystem: this.fb.control(false)
    });
    this.permissionsArray.push(group);
  }

  removePermission(i: number) {
    const group = this.permissionsArray.at(i);
    const existingId = group.get('existingId')?.value;

    this.permissionsArray.removeAt(i);

    if (existingId) {
      this.deletedPermissions.update(p => [...p, existingId]);
    }
  }

  ngOnInit() {
    this.detectViewMode();
  }

  private detectViewMode() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.currentGroupId.set(id);
      this.isEdit.set(true);
      this.loadExistingPermissionGroup();
    }
  }

  private loadExistingPermissionGroup() {
    this.showLoader();
    this.permissionsService.getGroup(this.currentGroupId()!)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorMessage = error.error?.detail;
          this.notificationService.showError('Permission not found', errorMessage);
          this.currentGroupId.set(null);
          this.isEdit.set(false);
          return of(null);
        }),
        tap((group) => {
          if (group) {
            this.isEdit.set(true);
            this.currentGroup.set(group);
            this.initializeFormFromGroup(group);
          }
        }),
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.hideLoader();
        })
      ).subscribe();
  }

  private initializeFormFromGroup(group: PermissionGroup) {
    this.form.patchValue({ name: group.name });
    group.permissions.forEach(p => {
      const group = this.fb.group({
        name: this.fb.control(p.name, [Validators.required]),
        existingId: this.fb.control(p.id, [Validators.required]),
        isSystem: this.fb.control(p.isSystem)
      });

      this.permissionsArray.push(group);
    });
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    const isEdit = this.isEdit() && !!this.currentGroup();
    const action = isEdit ? 'Updating' : 'Adding';
    const observable = isEdit ?
      this.updateGroup() : this.createGroup();

    this.form.disable();
    this.showLoader();
    observable.pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while ${action.toLowerCase()} permission`, errorMessage);
        return of(null);
      }),
      tap(async (result) => {
        if (result) {
          this.notificationService.showSuccess('OK', `Permission was ${isEdit ? 'updated': 'added'}.`);
          await this.router.navigate(['tenant', 'permissions']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  private createGroup() {
    const request: CreatePermissionRequest = {
      name: this.form.value.name!,
      permissions: this.permissionsArray.value
    };

    return this.permissionsService.createGroup(request);
  }

  private updateGroup() {
    const request: UpdatePermissionRequest = {
      name: this.form.value.name!,
      deletePermissions: this.deletedPermissions(),
      permissions: this.permissionsArray.value,
    };

    return this.permissionsService.updateGroup(this.currentGroupId()!, request);
  }
}
