using ErrorOr;
using Microsoft.Extensions.Configuration;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.SuperAdmin.Tenants.DeleteTenant;

public class DeleteTenantHandler(
    IBaseRepository<Tenant> tenantRepository,
    IConfiguration configuration) : ICommandHandler<DeleteTenantCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetAsync(
            t => t.Id == request.Id,
            cancellationToken: cancellationToken);

        if (tenant is null)
        {
            return Error.NotFound(TenantErrors.NotFound, TenantErrors.NotFoundDescription);
        }
        
        var superAdminEmail = configuration["SuperAdmin:Email"];
        if (tenant.Email.Equals(superAdminEmail, StringComparison.InvariantCultureIgnoreCase))
        {
            return Error.Failure(TenantErrors.DeleteSuperFailed, TenantErrors.DeleteSuperFailedDescription);
        }
        
        await tenantRepository.DeleteAsync(tenant, cancellationToken);
        
        return new Success();
    }
}