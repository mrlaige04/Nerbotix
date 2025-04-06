import {inject, Injectable} from '@angular/core';
import {BaseHttp} from '../base/base-http';
import {
  UpdateCategoryLinearOptimizationParamsRequest
} from '../../models/robots/categories/requests/update-category-linearopt-params-request';
import {Observable} from 'rxjs';
import {Success} from '../../models/success';
import {
  AntColonyAlgorithmSettings,
  GeneticAlgorithmSettings, LoadBalancingAlgorithmSettings, SimulatedAnnealingAlgorithmSettings, TenantAlgorithmSettings
} from '../../models/tenants/settings/tenant-algorithms-settings';

@Injectable({
  providedIn: 'root'
})
export class TenantSettingsService {
  private base = inject(BaseHttp);
  private readonly baseUrl = 'tenant-settings';

  getAlgorithmSettings(): Observable<TenantAlgorithmSettings> {
    const url = `${this.baseUrl}/algorithms`;
    return this.base.get<TenantAlgorithmSettings>(url);
  }

  updateGeneralAlgorithmSettings(data: { name: string }): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/general`;
    return this.base.put<{ name: string }, Success>(url, data);
  }

  updateAntColonyAlgorithm(data: AntColonyAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/ant-colony`;
    return this.base.put<AntColonyAlgorithmSettings, Success>(url, data);
  }

  updateGeneticAlgorithm(data: GeneticAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/genetic`;
    return this.base.put<GeneticAlgorithmSettings, Success>(url, data);
  }

  updateLoadBalancingAlgorithm(data: LoadBalancingAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/load-balancing`;
    return this.base.put<LoadBalancingAlgorithmSettings, Success>(url, data);
  }

  updateSimulatedAnnealingAlgorithm(data: SimulatedAnnealingAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/simulated-annealing`;
    return this.base.put<SimulatedAnnealingAlgorithmSettings, Success>(url, data);
  }

  updateLinearOptimizationAlgorithm(data: UpdateCategoryLinearOptimizationParamsRequest): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/linear-optimization`;
    return this.base.put<UpdateCategoryLinearOptimizationParamsRequest, Success>(url, data);
  }
}
