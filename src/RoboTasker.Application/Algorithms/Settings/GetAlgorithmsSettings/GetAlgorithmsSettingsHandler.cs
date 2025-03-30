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
            AntColony = new AntColonySettingsResponse
            {
                AntCount = settings.AntColonyAlgorithmSettings.AntCount,
                Alpha = settings.AntColonyAlgorithmSettings.Alpha,
                Beta = settings.AntColonyAlgorithmSettings.Beta,
                Evaporation = settings.AntColonyAlgorithmSettings.Evaporation,
                Iterations = settings.AntColonyAlgorithmSettings.Iterations,
            },
            Genetic = new GeneticSettingsResponse
            {
                Generations = settings.GeneticAlgorithmSettings.Generations,
                MutationRate = settings.GeneticAlgorithmSettings.MutationRate,
                PopulationSize = settings.GeneticAlgorithmSettings.PopulationSize,
            },
            LoadBalancing = new LoadBalancingSettingsResponse
            {
                ComplexityFactor = settings.LoadBalancingAlgorithmSettings.ComplexityFactor,
            },
            SimulatedAnnealing = new SimulatedAnnealingSettingsResponse
            {
                CoolingRate = settings.SimulatedAnnealingAlgorithmSettings.CoolingRate,
                InitialTemperature = settings.SimulatedAnnealingAlgorithmSettings.InitialTemperature,
                MinTemperature = settings.SimulatedAnnealingAlgorithmSettings.MinTemperature,
                IterationsPerTemp = settings.SimulatedAnnealingAlgorithmSettings.IterationsPerTemp,
            }
        };
    }
}