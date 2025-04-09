using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Application.Algorithms.Settings.LoadBalancing;

public class UpdateLoadBalancingSettingsHandler(
    ICurrentUser currentUser,
    ITenantRepository<TenantSettings> settingsRepository) : ICommandHandler<UpdateLoadBalancingSettingsCommand>
{
    public async Task<ErrorOr<Success>> Handle(UpdateLoadBalancingSettingsCommand request, CancellationToken cancellationToken)
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
        
        settings.AlgorithmSettings.LoadBalancingAlgorithmSettings.ComplexityFactor = request.ComplexityFactor;
        await settingsRepository.UpdateAsync(settings, cancellationToken);

        return new Success();
    }
}