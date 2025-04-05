import {Guid} from 'guid-typescript';
import {CreateCategoryPropertyRequest, } from './create-category-request';

export interface UpdateCategoryRequest {
  name?: string | undefined;
  deletedProperties?: Guid[] | undefined;
  newProperties?: CreateCategoryPropertyRequest[] | undefined;
}
