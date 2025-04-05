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
        
        settings.AntColonyAlgorithmSettings.AntCount = request.AntCount;
        settings.AntColonyAlgorithmSettings.Iterations = request.Iterations;
        settings.AntColonyAlgorithmSettings.Evaporation = request.Evaporation;
        settings.AntColonyAlgorithmSettings.Alpha = request.Alpha;
        settings.AntColonyAlgorithmSettings.Beta = request.Beta;
        await settingsRepository.UpdateAsync(settings, cancellationToken);
        
        return new Success();
    }
}