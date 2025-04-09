import {Guid} from 'guid-typescript';

export interface UpdateCategoryLinearOptimizationParamsRequest {
  isMaximization: boolean;
  categoryId: Guid;
  updateCategoryLinearParamsList: UpdateCategoryLinearOptimizationPropertyFactor[];
}

export interface UpdateCategoryLinearOptimizationPropertyFactor {
  propertyId: Guid;
  factor: number;
}
