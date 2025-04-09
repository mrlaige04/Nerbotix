import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {FormArray, FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {CapabilitiesService} from '../../../../services/robots/capabilities.service';
import {Capability} from '../../../../models/robots/capabilities/capability';
import {Guid} from 'guid-typescript';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {
  CreateCapabilityItemRequest,
  CreateCapabilityRequest
} from '../../../../models/robots/capabilities/requests/create-capability-request';
import {Tab, TabList, TabPanel, TabPanels, Tabs} from 'primeng/tabs';
import {InputText} from 'primeng/inputtext';
import {Button} from 'primeng/button';

@Component({
  selector: 'nb-capability-add-or-update',
  imports: [
    ReactiveFormsModule,
    Tabs,
    TabList,
    Tab,
    TabPanels,
    TabPanel,
    InputText,
    Button
  ],
  templateUrl: './capability-add-or-update.component.html',
  styleUrl: './capability-add-or-update.component.scss'
})
export class CapabilityAddOrUpdateComponent extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private capabilitiesService = inject(CapabilitiesService);

  form = this.fb.group({
    name: this.fb.control('', [Validators.required]),
    description: this.fb.control(''),
    capabilities: this.fb.array([])
  });

  capabilitiesArray = this.form.get('capabilities') as FormArray;

  isEdit = signal<boolean>(false);

  currentCapability = signal<Capability | null>(null);
  currentCapabilityId = signal<Guid | null>(null);

  deletedCapabilities = signal<Guid[] | undefined>(undefined);

  ngOnInit() {
    this.detectViewMode();
  }

  private detectViewMode() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.currentCapabilityId.set(id);
      this.loadExistingCapability();
    }
  }

  private loadExistingCapability() {
    this.showLoader();
    this.capabilitiesService.getCapabilityById(this.currentCapabilityId()!)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorMessage = error.error.detail;
          this.notificationService.showError('Capability not found', errorMessage);
          this.currentCapabilityId.set(null);
          return of(null);
        }),
        tap((capability) => {
          if (capability) {
            this.isEdit.set(true);
            this.currentCapability.set(capability);
            this.initializeFormFromCapability(capability);
          }
        }),
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.hideLoader();
        })
      ).subscribe();
  }

  private initializeFormFromCapability(capability: Capability) {
    this.form.patchValue({
      name: capability.groupName,
      description: capability.description
    });

    this.capabilitiesArray.clear();
    capability.capabilities.forEach(c => {
      const group = this.fb.group({
        name: this.fb.control(c.name, Validators.required),
        description: this.fb.control(c.description),
        existingId: this.fb.control(c.id)
      });

      this.capabilitiesArray.push(group);
    });
  }

  addNewCapability() {
    const group = this.fb.group({
      name: this.fb.control('', Validators.required),
      description: this.fb.control('')
    });

    this.capabilitiesArray.push(group);
  }

  removeCapability(i: number) {
    const group = this.capabilitiesArray.at(i);
    const name = group.get('name')?.value;

    this.capabilitiesArray.removeAt(i);

    if (this.isEdit() && this.currentCapability()) {
      const deletedCap = this.currentCapability()!.capabilities
        ?.find(c => c.name === name);
      if (deletedCap) {
        this.deletedCapabilities.update(c => [...c ?? [], deletedCap.id]);
      }
    }
  }

  submit()  {
    if (!this.form.valid) {
      return;
    }

    const isEdit = this.isEdit() && !!this.currentCapability();
    const action = isEdit ? 'Updating' : 'Adding';
    const observable = isEdit ?
      this.updateCapability() : this.createCapability();

    this.form.disable();
    this.showLoader();
    observable.pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while ${action.toLowerCase()} capability`, errorMessage);
        return of(null);
      }),
      tap(async (result) => {
        if (result) {
          this.notificationService.showSuccess('OK', `Capability was ${isEdit ? 'updated': 'added'}.`);
          await this.router.navigate(['capabilities']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  private createCapability() {
    const formValue = this.form.value;
    const capabilities = formValue.capabilities as CreateCapabilityItemRequest[];
    const request: CreateCapabilityRequest = {
      name: formValue.name!,
      capabilities: capabilities
    };

    return this.capabilitiesService.createCapability(request);
  }

  private updateCapability() {
    const formValue = this.form.value;
    const newCapabilities = this.capabilitiesArray.value
      ?.filter((cp: any) => cp.existingId == null);

    return this.capabilitiesService.updateCapability(this.currentCapabilityId()!, {
      name: formValue.name!,
      description: formValue.description!,
      deletedCapabilities: this.deletedCapabilities(),
      newCapabilities: newCapabilities
    });
  }
}
