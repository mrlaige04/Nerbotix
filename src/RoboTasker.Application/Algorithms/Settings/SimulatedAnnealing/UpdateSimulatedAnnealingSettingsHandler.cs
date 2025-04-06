using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Application.Algorithms.Settings.SimulatedAnnealing;

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