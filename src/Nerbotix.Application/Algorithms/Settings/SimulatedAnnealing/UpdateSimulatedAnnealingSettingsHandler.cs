using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Application.Algorithms.Settings.SimulatedAnnealing;

public class UpdateSimulatedAnnealingSettingsHandler(
    ICurrentUser currentUser,
    ITenantRepository<TenantSettings> settingsRepository) : ICommandHandler<UpdateSimulatedAnnealingSettingsCommand>
{
    public async Task<ErrorOr<Success>> Handle(UpdateSimulatedAnnealingSettingsCommand request, CancellationToken cancellationToken)
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
        
        settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.InitialTemperature = request.InitialTemperature;
        settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.CoolingRate = request.CoolingRate;
        settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.IterationsPerTemp = request.IterationsPerTemp;
        settings.AlgorithmSettings.SimulatedAnnealingAlgorithmSettings.MinTemperature = request.MinTemperature;
        await settingsRepository.UpdateAsync(settings, cancellationToken: cancellationToken);
        
        return new Success();
    }
}