using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Application.Algorithms.Settings.GetAlgorithmsSettings;

public class GetAlgorithmsSettingsHandler(
    ICurrentUser currentUser,
    ITenantRepository<TenantSettings> settingsRepository) : IQueryHandler<GetAlgorithmsSettingsQuery, TenantAlgorithmsSettingsResponse>
{
    public async Task<ErrorOr<TenantAlgorithmsSettingsResponse>> Handle(GetAlgorithmsSettingsQuery request, CancellationToken cancellationToken)
    {
        var tenantId = currentUser.GetTenantId();
        if (tenantId == null)
        {
            return Error.Failure(TenantErrors.NotFound, TenantErrors.NotFoundDescription);
        }
        
        var settings = await settingsRepository.GetAsync(cancellationToken: cancellationToken);
        if (settings == null)
        {
            return Error.Failure(TenantErrors.NotFound, TenantErrors.NotFoundDescription);
        }

        return new TenantAlgorithmsSettingsResponse
        {
            Id = settings.Id,
            PreferredAlgorithm = settings.AlgorithmSettings.PreferredAlgorithm,
            AntColony = new AntColonySettingsResponse
            {
                AntCount = settings.AlgorithmSettings.AntColonyAlgorithmSettings.AntCount,
                Alpha = settings.AlgorithmSettings.AntColonyAlgorithmSettings.Alpha,
                Beta = settings.AlgorithmSettings.AntColonyAlgorithmSettings.Beta,
                Evaporation = settings.AlgorithmSettings.AntColonyAlgorithmSettings.Evaporation,
                Iterations = settings.AlgorithmSettings.AntColonyAlgorithmSettings.Iterations,
            },
            Genetic = new GeneticSettingsResponse
            {
                Generations = settings.AlgorithmSettings.GeneticAlgorithmSettings.Generations,
                MutationRate = settings.AlgorithmSettings.GeneticAlgorithmSettings.MutationRate,
                PopulationSize = settings.AlgorithmSettings.GeneticAlgorithmSettings.PopulationSize,
            },
            LoadBalancing = new LoadBalancingSettingsResponse
            {
                ComplexityFactor = settings.AlgorithmSettings.LoadBalancingAlgorithmSettings.ComplexityFactor,
            },
            SimulatedAnnealing = new SimulatedAnnealingSettingsResponse
            {
                CoolingRate = settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.CoolingRate,
                InitialTemperature = settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.InitialTemperature,
                MinTemperature = settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.MinTemperature,
                IterationsPerTemp = settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.IterationsPerTemp,
            }
        };
    }
}