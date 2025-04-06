using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Algorithms;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants.Settings;

namespace RoboTasker.Application.Algorithms.Settings.GeneralSettings;

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