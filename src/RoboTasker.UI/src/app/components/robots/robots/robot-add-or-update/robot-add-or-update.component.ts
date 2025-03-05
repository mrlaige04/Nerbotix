import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {FormArray, FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {RobotsService} from '../../../../services/robots/robots.service';
import {CategoriesService} from '../../../../services/robots/categories.service';
import {CategoryBase} from '../../../../models/robots/categories/category-base';
import {catchError, debounceTime, finalize, forkJoin, of, switchMap, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Button} from 'primeng/button';
import {Tab, TabList, TabPanel, TabPanels, Tabs} from 'primeng/tabs';
import {InputText} from 'primeng/inputtext';
import {Select, SelectChangeEvent} from 'primeng/select';
import {Guid} from 'guid-typescript';
import {CategoryProperty} from '../../../../models/robots/categories/category-property';
import {CategoryPropertyType} from '../../../../models/robots/categories/category-property-type';
import {Checkbox, CheckboxChangeEvent} from 'primeng/checkbox';
import {InputNumber} from 'primeng/inputnumber';
import {Textarea} from 'primeng/textarea';
import {DatePicker} from 'primeng/datepicker';
import {
  CreateRobotCapabilityRequest,
  CreateRobotCustomPropertyRequest,
  CreateRobotPropertyRequest,
  CreateRobotRequest
} from '../../../../models/robots/robots/requests/create-robot-request';
import {HttpErrorResponse} from '@angular/common/http';
import {Robot} from '../../../../models/robots/robots/robot';
import {PropertyTypeHelper} from '../../../../utils/helpers/property-type-helper';
import {AsyncPipe, JsonPipe} from '@angular/common';
import {Category} from '../../../../models/robots/categories/category';
import {CapabilitiesService} from '../../../../services/robots/capabilities.service';
import {Tree} from 'primeng/tree';
import {Capability} from '../../../../models/robots/capabilities/capability';

@Component({
  selector: 'rb-robot-add-or-update',
  imports: [
    Button,
    Tabs,
    TabList,
    Tab,
    TabPanels,
    TabPanel,
    InputText,
    Select,
    ReactiveFormsModule,
    Checkbox,
    InputNumber,
    Textarea,
    DatePicker,
    JsonPipe,
    AsyncPipe,
    Tree,
    FormsModule
  ],
  templateUrl: './robot-add-or-update.component.html',
  styleUrl: './robot-add-or-update.component.scss'
})
export class RobotAddOrUpdateComponent extends BaseComponent implements OnInit {
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private robotsService = inject(RobotsService);
  private categoriesService = inject(CategoriesService);
  private capabilitiesService = inject(CapabilitiesService);

  categories = signal<CategoryBase[]>([]);
  capabilities = signal<Capability[]>([]);
  selectedCapabilityGroup = signal<Capability | null>(null);

  form = this.fb.group({
    name: this.fb.control('', Validators.required),
    categoryId: this.fb.control('', Validators.required),
    properties: this.fb.array([], [Validators.required]),
    customProperties: this.fb.array([]),
    capabilities: this.fb.array([])
  });

  categoryIdControl = this.form.get('categoryId') as FormControl;
  propertiesArray = this.form.get('properties') as FormArray;
  customPropertiesArray = this.form.get('customProperties') as FormArray;
  capabilitiesArray = this.form.get('capabilities') as FormArray;

  properties = signal<CategoryProperty[] | null>(null);

  isEdit = signal<boolean>(false);

  currentRobot = signal<Robot | null>(null);
  currentRobotId = signal<Guid | null>(null);

  deletedCustomProperties = signal<Guid[] | undefined>(undefined);
  categoryChanged = signal<boolean | undefined>(undefined);

  selectedCapabilityItems = signal<CreateRobotCapabilityRequest[]>([]);

  ngOnInit() {
    this.detectViewMode();

    this.getCategories();
    this.getCapabilities();
    this.listenForCategoryIdChange();
  }

  private detectViewMode() {
    const id = this.activatedRoute.snapshot.params['id'];
    if (id) {
      this.currentRobotId.set(id);
      this.loadExistingRobot();
    }
  }

  onCategoryChange(event: SelectChangeEvent) {
    if (this.currentRobot()?.category.id.toString() !== event.value.toString()) {
      this.categoryChanged.set(true);
    } else if (this.currentRobot()) {
      this.initializeFormFromRobot(this.currentRobot()!);
    }
  }

  private loadExistingRobot() {
    this.showLoader();
    this.robotsService.getRobotById(this.currentRobotId()!)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const errorMessage = error.error.detail;
          this.notificationService.showError('Robot not found', errorMessage);
          this.currentRobotId.set(null);
          return of(null);
        }),
        tap((robot) => {
          if (robot) {
            this.isEdit.set(true);
            this.currentRobot.set(robot);
            this.initializeFormFromRobot(robot);
          }
        }),
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.hideLoader();
        })
      ).subscribe();
  }

  private initializeFormFromRobot(robot: Robot) {
    this.form.patchValue({
      name: robot.name,
      categoryId: robot.category.id.toString(),
    });

    this.propertiesArray.clear();
    robot.properties.forEach(p => {
      const group = this.fb.group({
        propertyId: this.fb.control(p.propertyId, Validators.required),
        value: this.fb.control(PropertyTypeHelper.ConvertToExactType(p.type, p.value.toLowerCase()), Validators.required)
      });

      this.propertiesArray.push(group);
    })

    this.customPropertiesArray.clear();
    const customProperties = robot.customProperties ?? [];
    customProperties.forEach(cp => {
      const customProperty = this.fb.group({
        name: this.fb.control(cp.name, Validators.required),
        value: this.fb.control(cp.value, Validators.required),
        existingId: this.fb.control(cp.id)
      });
      this.customPropertiesArray.push(customProperty);
    });

    this.selectedCapabilityItems.set(robot.capabilities ?? []);
  }

  private listenForCategoryIdChange() {
    this.showLoader();
    this.categoryIdControl.valueChanges
      .pipe(
        debounceTime(300),
        tap((id) => {
          this.initializeCategoryProperties(id as Guid);
        }),
        takeUntilDestroyed(this.destroyRef),
        finalize(() => {
          this.hideLoader();
        })
      ).subscribe()
  }

  addCustomProperty() {
    const customProperty = this.fb.group({
      name: this.fb.control('', Validators.required),
      value: this.fb.control('', Validators.required),
      existingId: this.fb.control(null)
    });

    this.customPropertiesArray.push(customProperty);
  }

  removeCustomProperty(i: number) {
    const group = this.customPropertiesArray.at(i);
    const name = group.get('name')?.value;

    this.customPropertiesArray.removeAt(i);

    if (this.isEdit() && this.currentRobot()) {
      const deletedProp = this.currentRobot()!.customProperties
        ?.find(p => p.name === name);
      if (deletedProp) {
        this.deletedCustomProperties.update(p => [...p ?? [], deletedProp.id]);
      }
    }
  }

  private getCategories() {
    this.showLoader();
    this.categoriesService.getCategories({
      pageNumber: 1,
      pageSize: 999999
    }).pipe(
      tap((result) => {
        this.categories.set(result.items);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.hideLoader();
      })
    ).subscribe();
  }

  private getCapabilities() {
    this.showLoader();
    this.capabilitiesService.getCapabilities({
      pageNumber: 1,
      pageSize: 999999
    }).pipe(
      switchMap(base => {
        return forkJoin(base.items.map(c => this.capabilitiesService.getCapabilityById(c.id)))
      }),
      tap((result) => {
        if (result.length > 0) {
          this.selectedCapabilityGroup.set(result[0]);
        }

        this.capabilities.set(result.filter(c => !!c));
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.hideLoader();
      })
    ).subscribe();
  }

  getIsSelectedItem(groupId: Guid, id: Guid) {
    const items = this.selectedCapabilityItems();

    const el = items.find(
      i => i.groupId == groupId && i.id == id
    );

    return !!el;
  }

  private initializeCategoryProperties(id: Guid) {
    this.categoriesService.getCategoryById(id)
      .pipe(
        tap((result) => {
          if (result) {
            this.properties.set(result.properties);
            if (id != this.currentRobot()?.category.id) {
              this.initClearPropertiesForm(result);
            }
          }
        }),
        takeUntilDestroyed(this.destroyRef),
      ).subscribe();
  }

  private initClearPropertiesForm(category: Category) {
    this.propertiesArray.clear();
    category.properties.forEach(c => {
      const defaultValue = c.type == CategoryPropertyType.Boolean ? false : '';
      const property = this.fb.group({
        propertyId: this.fb.control(c.id, Validators.required),
        value: this.fb.control(defaultValue, Validators.required),
      });

      this.propertiesArray.push(property);
    });
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    const isEdit = this.isEdit() && !!this.currentRobot();
    const action = isEdit ? 'Updating' : 'Adding';
    const observable = isEdit ?
      this.updateRobot() : this.createRobot();

    this.form.disable();
    this.showLoader();
    observable.pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while ${action.toLowerCase()} robot`, errorMessage);
        return of(null);
      }),
      tap(async (result) => {
        if (result) {
          this.notificationService.showSuccess('OK', `Robot was ${isEdit ? 'updated': 'added'}.`);
          await this.router.navigate(['robots']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  private createRobot() {
    const formValue = this.form.value;
    const properties = formValue.properties as CreateRobotPropertyRequest[];
    const customProperties = formValue.customProperties as CreateRobotCustomPropertyRequest[];

    const request: CreateRobotRequest = {
      name: formValue.name!,
      categoryId: Guid.parse(formValue.categoryId!).toString(),
      properties: properties,
      customProperties: customProperties,
      capabilities: this.selectedCapabilityItems()
    };

    return this.robotsService.createRobot(request);
  }

  private updateRobot() {
    const formValue = this.form.value;
    const updatedProperties = this.propertiesArray.value;
    const newCustomProperties = this.customPropertiesArray.value
      ?.filter((cp: any) => cp.existingId === null);

    const categoryId = formValue.categoryId!;

    const currentCapabilities = this.selectedCapabilityItems();
    const startCapabilities = this.currentRobot()?.capabilities ?? [];

    const deletedCapabilities = startCapabilities.filter(c =>
        !currentCapabilities.includes(c));
    const newCapabilities = currentCapabilities.filter(c =>
        !startCapabilities.includes(c),);

    return this.robotsService.updateRobot(this.currentRobotId()!, {
      name: formValue.name!,
      deletedCustomProperties: this.deletedCustomProperties(),
      categoryId,
      newCustomProperties,
      updatedProperties,
      deletedCapabilities,
      newCapabilities,
    });
  }

  onCapabilityItemSelect(event: CheckboxChangeEvent, groupId: Guid, id: Guid) {
    const existingElement = this.selectedCapabilityItems()
      .find(i => i.groupId === groupId && i.id == id);

    if (event.checked && !existingElement) {
      this.selectedCapabilityItems.update(i => [...i, {
        groupId, id
      }]);
    } else if (!event.checked && existingElement) {
      const items = this.selectedCapabilityItems();
      this.selectedCapabilityItems.set(items.filter(i => i != existingElement));
    }
  }

  getProperty(propertyId: Guid): CategoryProperty | undefined {
    return this.properties()?.find(p => p.id === propertyId);
  }

  protected readonly CategoryPropertyType = CategoryPropertyType;
}
