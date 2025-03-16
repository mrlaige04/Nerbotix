using MediatR;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Common.Behaviours;

public class TenantAuthBehaviour<TRequest, TResponse>(
    ICurrentUser currentUser,
    IBaseRepository<Tenant> tenantRepository)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : ITenantRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var tenantId = currentUser.GetTenantId();
        if (!tenantId.HasValue || tenantId.Value == Guid.Empty)
        {
            throw new UnauthorizedAccessException();
        }

        if (!await tenantRepository.ExistsAsync(
                t => t.Id == tenantId.Value,
                cancellationToken: cancellationToken))
        {
            throw new UnauthorizedAccessException();
        }
        
        return await next();
    }
}