import {Guid} from 'guid-typescript';

export interface UpdateCategoryLinearOptimizationParamsRequest {
  isMaximization: boolean;
  updateCategoryLinearParamsList: UpdateCategoryLinearOptimizationPropertyFactor[];
}

export interface UpdateCategoryLinearOptimizationPropertyFactor {
  propertyId: Guid;
  factor: number;
}
