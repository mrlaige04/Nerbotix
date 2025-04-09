using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Algorithms;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Application.Algorithms.Settings.GeneralSettings;

public class UpdateGeneralAlgorithmSettingsHandler(
    ICurrentUser currentUser,
    ITenantRepository<TenantSettings> settingsRepository) : ICommandHandler<UpdateGeneralAlgorithmSettingsCommand>
{
    public async Task<ErrorOr<Success>> Handle(UpdateGeneralAlgorithmSettingsCommand request, CancellationToken cancellationToken)
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

        var algorithm = AlgorithmNames.GetAll().FirstOrDefault(
            a => a.Equals(request.Name, StringComparison.InvariantCultureIgnoreCase));

        if (string.IsNullOrEmpty(algorithm))
        {
            return Error.Failure(TenantErrors.NotFound, TenantErrors.NotFoundDescription);
        }
        
        settings.AlgorithmSettings.PreferredAlgorithm = algorithm;
        await settingsRepository.UpdateAsync(settings, cancellationToken);

        return new ErrorOr<Success>();
    }
}