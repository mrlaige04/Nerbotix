import {Component, computed, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {FormArray, FormBuilder, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {TasksService} from '../../../../services/robots/tasks.service';
import {CategoriesService} from '../../../../services/robots/categories.service';
import {CapabilitiesService} from '../../../../services/robots/capabilities.service';
import {CategoryBase} from '../../../../models/robots/categories/category-base';
import {Task} from '../../../../models/robots/tasks/task';
import {Guid} from 'guid-typescript';
import {catchError, finalize, forkJoin, of, tap} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {CreateTaskDataRequest, CreateTaskRequest} from '../../../../models/robots/tasks/requests/create-task-request';
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
import {DatePicker} from 'primeng/datepicker';
import {FileSelectEvent, FileUpload} from 'primeng/fileupload';
import {Divider} from 'primeng/divider';
import {TaskFile} from '../../../../models/robots/tasks/task-file';
import {Message} from 'primeng/message';
import {Checkbox} from 'primeng/checkbox';
import {PropertyTypeHelper} from '../../../../utils/helpers/property-type-helper';

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
    DatePicker,
    FileUpload,
    Divider,
    Message,
    Checkbox,
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
    name: this.fb.control('', [Validators.required, Validators.maxLength(50)]),
    description: this.fb.control(''),
    categoryId: this.fb.control('', [Validators.required]),
    priority: this.fb.control(0, [Validators.required, Validators.min(1), Validators.max(10)]),
    complexity: this.fb.control(0, [Validators.required, Validators.min(1), Validators.max(5)]),
    estimatedDuration: this.fb.control('000:00', [Validators.required, CustomValidators.Duration()]),
    requirements: this.fb.array([]),
    data: this.fb.array([])
  });

  requirementsArray = this.form.get('requirements') as FormArray;
  dataArray = this.form.get('data') as FormArray;

  categories = signal<CategoryBase[]>([]);
  capabilities = signal<CapabilityItem[]>([]);
  usedCapabilities = signal<Guid[]>([]);
  groupedCapabilities = computed(() =>
    this.groupCapabilities(this.capabilities()));

  allAdded = computed(() => {
    const capabilities = this.capabilities().map(c => c.id).sort();
    const usedCapabilities = this.usedCapabilities().sort();
    return usedCapabilities.length === capabilities.length && capabilities.every(
      c => usedCapabilities.includes(c)
    );
  })

  requirementsLevels = EnumHelper.toArray(TaskRequirementLevel);

  groupCapabilities(items: CapabilityItem[]): SelectItemGroup<CapabilityItem>[] {
    const groupedMap = new Map<string, CapabilityItem[]>();

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

  existingFiles = signal<TaskFile[]>([]);
  uploadedFiles = signal<File[]>([]);
  deletedFiles = signal<string[]>([]);

  deletedRequirements = signal<Guid[]>([]);
  deletedData = signal<Guid[]>([]);

  isEdit = signal<boolean>(false);

  currentTask = signal<Task | null>(null);
  currentTaskId = signal<Guid | null>(null);

  ngOnInit() {
    const isEdit = this.detectViewMode();

    if (isEdit) this.loadData();
    else this.getCapabilities();

    this.loadCategories();
  }

  private loadCategories() {
    this.categoriesService.getCategories({
      pageNumber: 1,
      pageSize: 99999
    }).pipe(
      tap((categories) => {
        this.categories.set(categories.items);
      }),
      takeUntilDestroyed(this.destroyRef)
    ).subscribe();
  }

  private loadData() {
    forkJoin([this.getCapabilitiesSource(), this.loadExistingTaskSource()])
      .pipe(
        tap(([capabilities, task]) => {
          this.capabilities.set(capabilities.items);
          if (task) {
            this.currentTask.set(task);
            this.initializeFormFromTask(task!);
          }
        })
      ).subscribe();
  }

  private detectViewMode() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.currentTaskId.set(id);
      this.isEdit.set(true);
      return true;
    }

    return false;
  }

  private loadExistingTaskSource() {
    return this.tasksService.getTaskById(this.currentTaskId()!)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorMessage = error.error?.detail;
          this.notificationService.showError('Robot not found', errorMessage);
          this.currentTaskId.set(null);
          this.isEdit.set(false);
          return of(null);
        }),
        tap((task) => {
          if (task) {
            this.isEdit.set(true);
            this.currentTask.set(task);
            this.initializeFormFromTask(task);
          }
        }),
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.hideLoader();
        })
      );
  }

  private initializeFormFromTask(task: Task) {
    this.form.patchValue({
      name: task.name,
      description: task.description,
      priority: task.priority,
      complexity: task.complexity,
    });

    let estimatedDuration = task.estimatedDuration?.toString();
    const parts = estimatedDuration?.split(':').map(n => parseInt(n, 10));

    if (parts && parts.length === 3) {
      let totalHours = parts[0]*24+parts[1];
      estimatedDuration = `${totalHours < 100 ? `0${totalHours}` : totalHours}:${parts[2]}`;
      this.form.patchValue({
        estimatedDuration: estimatedDuration,
      });
    }

    this.dataArray.clear();
    task.data?.forEach(d => {
      const val = d.type === TaskDataType.Boolean ? d.value?.toLowerCase() === 'true' : d.value;
      const group = this.fb.group({
        existingId: this.fb.control(d.id, Validators.required),
        key: this.fb.control(d.key, [Validators.required, Validators.maxLength(20)]),
        type: this.fb.control(d.type, [Validators.required]),
        value: this.fb.control(val, [Validators.required])
      });

      this.dataArray.push(group);
    });

    this.existingFiles.set(task.files ?? []);

    this.requirementsArray.clear();
    task.requirements?.forEach(r => {
      const capability = this.capabilities().find(c => c.id === r.capabilityId);
      const group = this.fb.group({
        existingId: this.fb.control(r.capabilityId, Validators.required),
        capabilityId: this.fb.control(capability, [Validators.required]),
        level: this.fb.control(r.level, [Validators.required])
      });

      this.requirementsArray.push(group);
    });
  }

  private getCapabilitiesSource() {
    return this.capabilitiesService.getCapabilitiesItems({
      pageNumber: 1,
      pageSize: 9999
    }).pipe(
      tap(result => {
        this.capabilities.set(result.items);
      }),
      takeUntilDestroyed(this.destroyRef)
    );
  }

  private getCapabilities() {
    this.getCapabilitiesSource().subscribe();
  }

  onCapabilitySelect(_: SelectChangeEvent) {
    this.usedCapabilities.set(
      this.requirementsArray.value.map((v: any) => v.capabilityId.id));
  }

  onDataTypeChange(i: number) {
    const group = this.dataArray.at(i);
    const valueControl = group.get('value');
    const isBoolean = group.get('type')?.value === TaskDataType.Boolean;
    valueControl?.setValue(!isBoolean ? null : false);
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
    const existingId = group.get('existingId')?.value;

    if (id && this.capabilities().some(c => c.id === id)) {
      this.usedCapabilities.update(u => u.filter(c => c !== id));
    }

    this.requirementsArray.removeAt(i);
    if (existingId) {
      this.deletedRequirements.update(r => [...r, existingId])
    }
  }

  addDataItem() {
    const group = this.fb.group({
      key: this.fb.control('', [Validators.required, Validators.maxLength(20)]),
      type: this.fb.control(TaskDataType.String, [Validators.required]),
      value: this.fb.control('', [Validators.required])
    });

    this.dataArray.push(group);
  }

  removeDataItem(i: number) {
    const group = this.dataArray.at(i);
    const existingId = group.get('existingId')?.value;

    this.dataArray.removeAt(i);

    if (existingId) {
      this.deletedData.update(d => [...d, existingId]);
    }
  }

  onFilesSelect(event: FileSelectEvent) {
    this.uploadedFiles.set(event.currentFiles);
  }

  deleteFile(name: string) {
    this.deletedFiles.update(f => [...f, name]);
  }

  undeleteFile(name: string) {
    this.deletedFiles.update(f => f.filter(c => c !== name));
  }

  isFileDeleted(name: string) {
    return this.deletedFiles().some(f => f === name);
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
      description: formValue.description,
      name: formValue.name,
      categoryId: Guid.parse(formValue.categoryId!),
      priority: formValue.priority,
      complexity: formValue.complexity,
      estimatedDuration: DateHelper.NormalizeDuration(formValue.estimatedDuration!),
      requirements: requirements,
      files: this.uploadedFiles(),
      data: formValue.data as CreateTaskDataRequest[],
    } as CreateTaskRequest;
    return this.tasksService.createTask(request);
  }

  private updateTask() {
    const formValue = this.form.value;
    const requirements = this.requirementsArray.value.map((r: any) => ({
      capabilityId: r.capabilityId.id,
      existingId: r.existingId,
      level: r.level
    }));

    const data = this.dataArray.value;
    return this.tasksService.updateTask(this.currentTaskId()!, {
      name: formValue.name!,
      description: formValue.description!,
      priority: formValue.priority!,
      complexity: formValue.complexity!,
      categoryId: Guid.parse(formValue.categoryId!),
      estimatedDuration: DateHelper.NormalizeDuration(formValue.estimatedDuration!),
      deletedRequirements: this.deletedRequirements(),
      deletedData: this.deletedData(),
      deletedFiles: this.deletedFiles(),
      requirements,
      data,
      files: this.uploadedFiles()
    });
  }

  priorityRange = ArrayHelper.CreateRange(1, 10);
  protected readonly TaskDataType = TaskDataType;
}
