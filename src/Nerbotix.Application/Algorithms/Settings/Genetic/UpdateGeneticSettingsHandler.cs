using ErrorOr;
using Nerbotix.Application.Algorithms.Settings.LoadBalancing;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Application.Algorithms.Settings.Genetic;

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

        settings.AlgorithmSettings.GeneticAlgorithmSettings.PopulationSize = request.PopulationSize;
        settings.AlgorithmSettings.GeneticAlgorithmSettings.MutationRate = request.MutationRate;
        settings.AlgorithmSettings.GeneticAlgorithmSettings.Generations = request.Generations;
        await settingsRepository.UpdateAsync(settings, cancellationToken);
        
        return new Success();
    }
}