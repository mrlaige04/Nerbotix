import {Component, computed, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {FormArray, FormBuilder, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {TasksService} from '../../../../services/robots/tasks.service';
import {CategoriesService} from '../../../../services/robots/categories.service';
import {CapabilitiesService} from '../../../../services/robots/capabilities.service';
import {CategoryBase} from '../../../../models/robots/categories/category-base';
import {CapabilityBase} from '../../../../models/robots/capabilities/capability-base';
import {Task} from '../../../../models/robots/tasks/task';
import {Guid} from 'guid-typescript';
import {catchError, finalize, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {
  CreateTaskDataRequest,
  CreateTaskPropertyRequest,
  CreateTaskRequest
} from '../../../../models/robots/tasks/requests/create-task-request';
import {Button} from 'primeng/button';
import {TaskRequirementLevel} from '../../../../models/robots/tasks/task-requirement-level';
import {TaskDataType} from '../../../../models/robots/tasks/task-data-type';
import {Tab, TabList, TabPanel, TabPanels, Tabs} from 'primeng/tabs';
import {InputText} from 'primeng/inputtext';
import {Textarea} from 'primeng/textarea';
import {InputNumber} from 'primeng/inputnumber';
import {InputMask} from 'primeng/inputmask';
import {CustomValidators} from '../../../../utils/validators/custom-validators';
import {Select, SelectChangeEvent} from 'primeng/select';
import {ArrayHelper} from '../../../../utils/helpers/array-helper';
import {DateHelper} from '../../../../utils/helpers/date-helper';
import {SelectItemGroup} from 'primeng/api';
import {CapabilityItem} from '../../../../models/robots/capabilities/capability-item';
import {EnumHelper} from '../../../../utils/helpers/enum-helper';
import {JsonPipe} from '@angular/common';
import {Checkbox} from 'primeng/checkbox';
import {DatePicker} from 'primeng/datepicker';
import {FileSelectEvent, FileUpload} from 'primeng/fileupload';
import {Divider} from 'primeng/divider';

@Component({
  selector: 'rb-tasks-add-or-update',
  imports: [
    Button,
    FormsModule,
    Tabs,
    TabList,
    Tab,
    TabPanels,
    TabPanel,
    InputText,
    ReactiveFormsModule,
    Textarea,
    InputNumber,
    InputMask,
    Select,
    JsonPipe,
    Checkbox,
    DatePicker,
    FileUpload,
    Divider
  ],
  templateUrl: './tasks-add-or-update.component.html',
  styleUrl: './tasks-add-or-update.component.scss'
})
export class TasksAddOrUpdateComponent extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private tasksService = inject(TasksService);
  private categoriesService = inject(CategoriesService);
  private capabilitiesService = inject(CapabilitiesService);

  form = this.fb.group({
    name: this.fb.control('', [Validators.required]),
    description: this.fb.control(''),
    priority: this.fb.control(0, [Validators.required, Validators.min(1), Validators.max(10)]),
    complexity: this.fb.control(0, [Validators.required, Validators.min(1), Validators.max(5)]),
    estimatedDuration: this.fb.control('00:00', [Validators.required, CustomValidators.Duration()]),
    properties: this.fb.array([]),
    requirements: this.fb.array([]),
    data: this.fb.array([])
  });

  propertiesArray = this.form.get('properties') as FormArray;
  requirementsArray = this.form.get('requirements') as FormArray;
  dataArray = this.form.get('data') as FormArray;

  categories = signal<CategoryBase[]>([]);
  capabilities = signal<CapabilityBase[]>([]);
  usedCapabilities = signal<Guid[]>([]);
  groupedCapabilities = computed(() =>  this.groupCapabilities(this.capabilities()));
  allAdded = computed(() => {
    const capabilities = this.capabilities().map(c => c.id).sort();
    const usedCapabilities = this.usedCapabilities().sort();

    return usedCapabilities.length === capabilities.length && capabilities.every(
      c => usedCapabilities.includes(c)
    );
  })

  requirementsLevels = EnumHelper.toArray(TaskRequirementLevel);

  groupCapabilities(items: CapabilityBase[]): SelectItemGroup<CapabilityBase>[] {
    const groupedMap = new Map<string, CapabilityBase[]>();

    items.forEach(item => {
      if (!groupedMap.has(item.groupName)) {
        groupedMap.set(item.groupName, []);
      }
      groupedMap.get(item.groupName)!.push(item);
    });

    return Array.from(groupedMap.entries()).map(([groupName, groupItems]) => ({
      label: groupName,
      items: groupItems.map(item => ({
        label: item.name,
        value: item,
        disabled: this.usedCapabilities().includes(item.id)
      }))
    }));
  }

  dataTypes = EnumHelper.toArray(TaskDataType);

  uploadedFiles = signal<File[]>([]);

  isEdit = signal<boolean>(false);

  currentTask = signal<Task | null>(null);
  currentTaskId = signal<Guid | null>(null);

  ngOnInit() {
    this.detectViewMode();
    this.getCapabilities();
  }

  private detectViewMode() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.isEdit.set(true);
    }
  }

  private getCapabilities() {
    this.capabilitiesService.getCapabilities({
      pageNumber: 1,
      pageSize: 99999
    }).pipe(
      tap(result => {
        this.capabilities.set(result.items);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  addProperty() {
    if (this.usedCapabilities().length === this.capabilities().length) {
      return;
    }

    const prop = this.fb.group({
      key: this.fb.control('', [Validators.required]),
      value: this.fb.control('', [Validators.required])
    });

    this.propertiesArray.push(prop);
  }

  removeProperty(i: number) {
    const group = this.propertiesArray.at(i);
    const key = group.get('key')?.value;
    this.propertiesArray.removeAt(i);

    // TODO: Add to delete array
  }

  onCapabilitySelect(_: SelectChangeEvent) {
    this.usedCapabilities.set(
      this.requirementsArray.value.map((v: any) => v.capabilityId.id));
  }

  addRequirement() {
    const group = this.fb.group({
      capabilityId: this.fb.control('', [Validators.required]),
      level: this.fb.control(TaskRequirementLevel.Mandatory, [Validators.required])
    });

    this.requirementsArray.push(group);
  }

  removeRequirement(i: number) {
    const group = this.requirementsArray.at(i);
    const key = group.get('capabilityId')?.value;
    const id = key['id'];

    if (id && this.capabilities().some(c => c.id === id)) {
      this.usedCapabilities.update(u => u.filter(c => c !== id));
    }

    this.requirementsArray.removeAt(i);
    // TODO: remove from selected capabilities array
  }

  addDataItem() {
    const group = this.fb.group({
      key: this.fb.control('', [Validators.required]),
      type: this.fb.control(TaskDataType.String, [Validators.required]),
      value: this.fb.control('', [Validators.required])
    });

    this.dataArray.push(group);
  }

  removeDataItem(i: number) {
    const group = this.dataArray.at(i);
    this.dataArray.removeAt(i);
  }

  onFilesSelect(event: FileSelectEvent) {
    this.uploadedFiles.set(event.currentFiles);
  }

  submit() {
    const isEdit = this.isEdit() && !!this.currentTask();
    const action = isEdit ? 'Updating' : 'Adding';
    const observable = isEdit ?
      this.updateTask() : this.createTask();

    this.form.disable();
    this.showLoader();
    observable.pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while ${action.toLowerCase()} task`, errorMessage);
        return of(null);
      }),
      tap(async (result) => {
        if (result) {
          this.notificationService.showSuccess('OK', `Task was ${isEdit ? 'updated': 'added'}.`);
          await this.router.navigate(['tasks']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  private createTask() {
    const formValue = this.form.value;
    const requirements = formValue.requirements?.map((r: any) => ({
      capabilityId: r.capabilityId.id,
      level: r.level
    }));

    const request: CreateTaskRequest = {
      name: formValue.name,
      priority: formValue.priority,
      complexity: formValue.complexity,
      estimatedDuration: DateHelper.NormalizeDuration(formValue.estimatedDuration!),
      properties: formValue.properties as CreateTaskPropertyRequest[],
      requirements: requirements,
      files: this.uploadedFiles(),
      data: formValue.data as CreateTaskDataRequest[],
    } as CreateTaskRequest;
    return this.tasksService.createTask(request);
  }

  private updateTask() {
    return this.tasksService.deleteTask(Guid.create())
  }

  priorityRange = ArrayHelper.CreateRange(1, 10);
  protected readonly TaskDataType = TaskDataType;
}
