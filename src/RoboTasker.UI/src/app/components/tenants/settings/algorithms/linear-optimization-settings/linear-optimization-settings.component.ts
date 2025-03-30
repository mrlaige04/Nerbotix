import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {BaseComponent} from '../../../../common/base/base.component';
import {Guid} from 'guid-typescript';
import {CategoriesService} from '../../../../../services/robots/categories.service';
import {CategoryBase} from '../../../../../models/robots/categories/category-base';
import {catchError, finalize, of, tap} from 'rxjs';
import {takeUntilDestroyed} from '@angular/core/rxjs-interop';
import {Select, SelectChangeEvent} from 'primeng/select';
import {Category} from '../../../../../models/robots/categories/category';
import {HttpErrorResponse} from '@angular/common/http';
import {FormArray, FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {InputNumber} from 'primeng/inputnumber';
import {CategoryPropertyType} from '../../../../../models/robots/categories/category-property-type';
import {Button} from 'primeng/button';
import {
  UpdateCategoryLinearOptimizationParamsRequest, UpdateCategoryLinearOptimizationPropertyFactor
} from '../../../../../models/robots/categories/requests/update-category-linearopt-params-request';

@Component({
  selector: 'rb-linear-optimization-settings',
  imports: [
    Select,
    ReactiveFormsModule,
    InputNumber,
    Button
  ],
  templateUrl: './linear-optimization-settings.component.html',
  styleUrl: './linear-optimization-settings.component.scss'
})
export class LinearOptimizationSettingsComponent extends BaseComponent implements OnInit {
  private categoriesService = inject(CategoriesService);
  private destroyRef = inject(DestroyRef);
  private fb = inject(FormBuilder);

  categories = signal<CategoryBase[]>([]);
  categoryId = signal<Guid | null>(null);

  form = this.fb.group({
    isMaximization: this.fb.control(true, Validators.required),
    properties: this.fb.array([])
  });

  propertiesArray = this.form.get('properties') as FormArray;

  ngOnInit() {
    this.loadCategories();
  }

  private loadCategories() {
    this.showLoader();
    this.categoriesService.getCategories({
      pageNumber: 1,
      pageSize: 9999
    }).pipe(
      tap((categories) => {
        this.categories.set(categories.items);
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }

  onCategoryChange(event: SelectChangeEvent) {
    this.categoryId.set(event.value);
    this.loadCategory();
  }

  private loadCategory() {
    this.showLoader();
    this.categoriesService.getCategoryById(this.categoryId()!).pipe(
      catchError((error: HttpErrorResponse) => {
        const detail = error.error.detail;
        this.notificationService.showError('Error while loading properties', detail);
        return of(null);
      }),
      tap((category) => {
        if (!category) {
          this.categoryId.set(null);
          return;
        }

        this.form.patchValue({
          isMaximization: category.isMaximization === true
        });

        this.propertiesArray.clear();
        category.properties
          .filter(p => p.type === CategoryPropertyType.Number)
          .forEach(p => {
            const group = this.fb.group({
              name: this.fb.control(p.name),
              propertyId: this.fb.control(p.id, [Validators.required]),
              factor: this.fb.control(p.factor, [Validators.required]),
            });

            this.propertiesArray.push(group);
          });
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }

  submit() {
    if (!this.form.valid) {
      return;
    }

    const formValue = this.form.value;
    const request: UpdateCategoryLinearOptimizationParamsRequest = {
      isMaximization: formValue.isMaximization === true,
      updateCategoryLinearParamsList: formValue.properties as UpdateCategoryLinearOptimizationPropertyFactor[]
    };

    this.showLoader();
    this.categoriesService.updateLinearOptimizationAlgoParams(this.categoryId()!, request).pipe(
      catchError((error: HttpErrorResponse) => {
        const detail = error.error.detail;
        this.notificationService.showError('Error while loading properties', detail);
        return of(null);
      }),
      tap((success) => {
        if (success) {
          this.notificationService.showSuccess('OK', `Params were updated.`);
          this.router.navigate(['tenant/settings']);
        }
      }),
      takeUntilDestroyed(this.destroyRef),
      finalize(() => this.hideLoader())
    ).subscribe();
  }
}
