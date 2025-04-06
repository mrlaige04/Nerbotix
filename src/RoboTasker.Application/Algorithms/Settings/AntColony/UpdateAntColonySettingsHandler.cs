using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Application.Algorithms.Settings.AntColony;

public class UpdateAntColonySettingsHandler(
    ICurrentUser currentUser,
    ITenantRepository<TenantSettings> settingsRepository) : ICommandHandler<UpdateAntColonySettingsCommand>
{
    public async Task<ErrorOr<Success>> Handle(UpdateAntColonySettingsCommand request, CancellationToken cancellationToken)
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
        
        settings.AlgorithmSettings.AntColonyAlgorithmSettings.AntCount = request.AntCount;
        settings.AlgorithmSettings.AntColonyAlgorithmSettings.Iterations = request.Iterations;
        settings.AlgorithmSettings.AntColonyAlgorithmSettings.Evaporation = request.Evaporation;
        settings.AlgorithmSettings.AntColonyAlgorithmSettings.Alpha = request.Alpha;
        settings.AlgorithmSettings.AntColonyAlgorithmSettings.Beta = request.Beta;
        await settingsRepository.UpdateAsync(settings, cancellationToken);
        
        return new Success();
    }
}