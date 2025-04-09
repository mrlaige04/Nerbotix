using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Application.Algorithms.Settings.AntColony;

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