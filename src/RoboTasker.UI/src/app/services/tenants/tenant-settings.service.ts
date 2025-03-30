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

  getSettings(): Observable<TenantAlgorithmSettings> {
    const url = `${this.baseUrl}/algorithms`;
    return this.base.get<TenantAlgorithmSettings>(url);
  }

  updateAntColony(data: AntColonyAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/ant-colony`;
    return this.base.put<AntColonyAlgorithmSettings, Success>(url, data);
  }

  updateGenetic(data: GeneticAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/genetic`;
    return this.base.put<GeneticAlgorithmSettings, Success>(url, data);
  }

  updateLoadBalancing(data: LoadBalancingAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/load-balancing`;
    return this.base.put<LoadBalancingAlgorithmSettings, Success>(url, data);
  }

  updateSimulatedAnnealing(data: SimulatedAnnealingAlgorithmSettings): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/simulated-annealing`;
    return this.base.put<SimulatedAnnealingAlgorithmSettings, Success>(url, data);
  }

  updateLinearOptimizationAlgoParams(data: UpdateCategoryLinearOptimizationParamsRequest): Observable<Success> {
    const url = `${this.baseUrl}/algorithms/linear-optimization`;
    return this.base.put<UpdateCategoryLinearOptimizationParamsRequest, Success>(url, data);
  }
}
