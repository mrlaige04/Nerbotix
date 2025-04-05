export class TenantAlgorithmSettings {
  antColony!: AntColonyAlgorithmSettings;
  genetic!: GeneticAlgorithmSettings;
  loadBalancing!: LoadBalancingAlgorithmSettings;
  simulatedAnnealing!: SimulatedAnnealingAlgorithmSettings;
}

export class AntColonyAlgorithmSettings {
  antCount!: number;
  iterations!: number;
  evaporation!: number;
  alpha!: number;
  beta!: number;
}

export class GeneticAlgorithmSettings {
  populationSize!: number;
  generations!: number;
  mutationRate!: number;
}

export class LoadBalancingAlgorithmSettings {
  complexityFactor!: number;
}

export class SimulatedAnnealingAlgorithmSettings {
  initialTemperature!: number;
  coolingRate!: number;
  iterationsPerTemp!: number;
  minTemperature!: number;
}
