using MediatR;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Common.Behaviours;

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