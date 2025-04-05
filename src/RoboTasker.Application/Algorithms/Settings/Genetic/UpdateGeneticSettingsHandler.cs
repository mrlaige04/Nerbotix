using ErrorOr;
using RoboTasker.Application.Algorithms.Settings.LoadBalancing;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Application.Algorithms.Settings.Genetic;

public class UpdateGeneticSettingsHandler(
    ICurrentUser currentUser,
    ITenantRepository<TenantSettings> settingsRepository) : ICommandHandler<UpdateGeneticSettingsCommand>
{
    public async Task<ErrorOr<Success>> Handle(UpdateGeneticSettingsCommand request, CancellationToken cancellationToken)
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

        settings.GeneticAlgorithmSettings.PopulationSize = request.PopulationSize;
        settings.GeneticAlgorithmSettings.MutationRate = request.MutationRate;
        settings.GeneticAlgorithmSettings.Generations = request.Generations;
        await settingsRepository.UpdateAsync(settings, cancellationToken);
        
        return new Success();
    }
}