export enum AlgorithmCategory {
  Classical,
  MathBased,
  Heuristic,
  AI
}

export class AlgorithmName {
  name!: string;
  displayName?: string;
  description?: string;
  type!: AlgorithmCategory;

  // Classical
  public static LoadBalancing: AlgorithmName = {
    name: 'LoadBalancing',
    type: AlgorithmCategory.Classical,
    displayName: 'Load Balancing',
    description: ''
  };

  public static LatestTaskFinished: AlgorithmName = {
    name: 'LatestTaskFinished',
    type: AlgorithmCategory.Classical,
    displayName: 'Latest Task Finished',
    description: ''
  };

  public static RoundRobin: AlgorithmName = {
    name: 'RoundRobin',
    type: AlgorithmCategory.Classical,
    displayName: 'Round Robin',
    description: ''
  };

  public static LeastConnections: AlgorithmName = {
    name: 'LeastConnections',
    type: AlgorithmCategory.Classical,
    displayName: 'Least Connections',
    description: ''
  };

  public static Random: AlgorithmName = {
    name: 'Random',
    type: AlgorithmCategory.Classical,
    description: ''
  };


  // Math-Based
  public static LinearOptimization: AlgorithmName = {
    name: 'LinearOptimization',
    type: AlgorithmCategory.MathBased,
    displayName: 'Linear Optimization',
    description: ''
  };

  public static FuzzyLogic: AlgorithmName = {
    name: 'FuzzyLogic',
    type: AlgorithmCategory.MathBased,
    displayName: 'Fuzzy Logic',
    description: ''
  };

  public static AssignmentProblem: AlgorithmName = {
    name: 'AssignmentProblem',
    type: AlgorithmCategory.MathBased,
    displayName: 'Assignment Problem',
    description: ''
  };

  // Heuristic
  public static GeneticTask: AlgorithmName = {
    name: 'GeneticTask',
    type: AlgorithmCategory.Heuristic,
    displayName: 'Genetic Task',
    description: ''
  };

  public static AntColony: AlgorithmName = {
    name: 'AntColony',
    type: AlgorithmCategory.Heuristic,
    displayName: 'Ant Colony',
    description: ''
  };

  public static SimulatedAnnealing: AlgorithmName = {
    name: 'SimulatedAnnealing',
    type: AlgorithmCategory.Heuristic,
    displayName: 'Simulated Annealing',
    description: ''
  };

  public static get All(): AlgorithmName[] {
    return Object.values(AlgorithmName);
  }
}
