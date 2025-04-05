import {CategoryPropertyType} from '../category-property-type';

export interface CreateCategoryRequest {
  name: string;
  properties: CreateCategoryPropertyRequest[];
}

export interface CreateCategoryPropertyRequest {
  name: string;
  type: CategoryPropertyType;
}
