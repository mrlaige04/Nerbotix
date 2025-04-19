import {CategoryPropertyType} from '../category-property-type';
import {Guid} from 'guid-typescript';

export interface CreateCategoryRequest {
  name: string;
  properties: CreateCategoryPropertyRequest[];
}

export interface CreateCategoryPropertyRequest {
  name: string;
  type: CategoryPropertyType;
  existingId?: Guid;
}
