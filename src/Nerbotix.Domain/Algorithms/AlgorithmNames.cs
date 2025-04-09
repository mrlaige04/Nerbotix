using System.Reflection;

namespace Nerbotix.Domain.Algorithms;

public class AlgorithmNames
{
    // Classical
    public const string LoadBalancing = nameof(LoadBalancing);
    public const string LatestTaskFinished = nameof(LatestTaskFinished);
    public const string RoundRobin = nameof(RoundRobin);
    public const string LeastConnections = nameof(LeastConnections);
    public const string Random = nameof(Random);
    
    // Math-based
    public const string LinearOptimization = nameof(LinearOptimization);
    public const string FuzzyLogic = nameof(FuzzyLogic);
    public const string AssignmentProblem = nameof(AssignmentProblem);
    
    // Heuristic
    public const string GeneticTask = nameof(GeneticTask);
    public const string AntColony = nameof(AntColony);
    public const string SimulatedAnnealing = nameof(SimulatedAnnealing);
    
    public static string[] GetAll()
    {
        var fields = typeof(AlgorithmNames).GetFields(
            BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        
        return fields.Select(f => f.Name).ToArray();
    }
}