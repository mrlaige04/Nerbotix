import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../common/base/base.component';
import {Button} from 'primeng/button';
import {InputText} from 'primeng/inputtext';
import {FormArray, FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {Select} from 'primeng/select';
import {
  CategoryPropertyType,
} from '../../../../models/robots/categories/category-property-type';
import {EnumHelper} from '../../../../utils/helpers/enum-helper';
import {CreateCategoryRequest} from '../../../../models/robots/categories/requests/create-category-request';
import {CategoriesService} from '../../../../services/robots/categories.service';
import {catchError, finalize, of, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {HttpErrorResponse} from '@angular/common/http';
import {DynamicDialogConfig, DynamicDialogRef} from 'primeng/dynamicdialog';
import {Guid} from 'guid-typescript';
import {Category} from '../../../../models/robots/categories/category';

@Component({
  selector: 'nb-category-add-or-update',
  imports: [
    Button,
    InputText,
    ReactiveFormsModule,
    Select
  ],
  templateUrl: './category-add-or-update.component.html',
  styleUrl: './category-add-or-update.component.scss'
})
export class CategoryAddOrUpdateComponent extends BaseComponent implements OnInit{
  private fb = inject(FormBuilder);
  private destroyRef = inject(DestroyRef);
  private categoriesService = inject(CategoriesService);
  private dialogRef = inject(DynamicDialogRef<CategoryAddOrUpdateComponent>);
  private dialogConfig = inject(DynamicDialogConfig);

  form = this.fb.group({
    name: this.fb.control('', Validators.required),
    properties: this.fb.array([])
  });

  isEdit = signal<boolean>(false);
  currentCategoryId = signal<Guid | undefined>(undefined);
  currentCategory = signal<Category | undefined>(undefined);

  deletedProperties = signal<Guid[] | null>(null);

  propertiesArray = this.form.get('properties') as FormArray;

  ngOnInit() {
    this.detectViewMode();
  }

  private detectViewMode() {
    if (!this.dialogConfig.data) {
      return;
    }

    const isEdit = this.dialogConfig.data['isEdit'];
    const id = this.dialogConfig.data['id'];

    if (isEdit === true && id) {
      this.isEdit.set(true);
      this.currentCategoryId.set(id);

      this.loadCategory(id);
    }
  }

  private loadCategory(id: Guid) {
    this.categoriesService.getCategoryById(id)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          const detail = error.error.detail;
          this.notificationService.showError('Error while retrieving category', detail);
          return of(null);
        }),
        tap(category => {
          if (category) {
            this.currentCategory.set(category);
            this.form.patchValue(category);

            category.properties.forEach(p => {
              const property = this.fb.group({
                name: this.fb.control(p.name, Validators.required),
                type: this.fb.control(p.type, Validators.required),
                unit: this.fb.control(p.unit),
                existingId: this.fb.control(p.id)
              });
              this.propertiesArray.push(property);
            });
          }
        })
      ).subscribe();
  }

  addNewProperty() {
    const group = this.fb.group({
      name: this.fb.control('', Validators.required),
      type: this.fb.control('', Validators.required),
      unit: this.fb.control(null),
      existingId: this.fb.control(null)
    });

    this.propertiesArray.push(group);
  }

  removeProperty(index: number) {
    const group = this.propertiesArray.at(index);
    const name = group.get('name')?.value;

    this.propertiesArray.removeAt(index);

    if (this.isEdit() && this.currentCategoryId()) {
      const deletedProp = this.currentCategory()!.properties
        ?.find(p => p.name === name);
      if (deletedProp) {
        this.deletedProperties.update(p => [...p ?? [], deletedProp.id]);
      }
    }
  }

  propertyTypes = EnumHelper.toArray(CategoryPropertyType);

  submit() {
    if (!this.form.valid) {
      return;
    }

    const isEdit = this.isEdit() && this.currentCategory();
    const action = isEdit ? 'Updating' : 'Adding';
    const observable = isEdit ?
      this.updateCategory() : this.createCategory();

    this.form.disable();
    this.showLoader();
    observable.pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = error.error.detail;
        this.notificationService.showError(`Error while ${action} category`, errorMessage);
        return of(null);
      }),
      tap((result) => {
        if (result) {
          this.notificationService.showSuccess('OK', `Category was ${isEdit ? 'updated' : 'created'}.`);
          this.dialogRef.close(true);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => {
        this.form.enable();
        this.hideLoader();
      })
    ).subscribe();
  }

  private createCategory() {
    const request = this.form.value as CreateCategoryRequest;
    return this.categoriesService.createCategory(request);
  }

  private updateCategory() {
    const formValue = this.form.value;
    const newProperties = this.propertiesArray.value;

    return this.categoriesService.updateCategory(this.currentCategoryId()!, {
      name: formValue.name!,
      deletedProperties: this.deletedProperties() ?? [],
      newProperties
    });
  }
}
