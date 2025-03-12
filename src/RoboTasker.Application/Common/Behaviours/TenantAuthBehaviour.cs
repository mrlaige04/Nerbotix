using MediatR;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Services;

namespace RoboTasker.Application.Common.Behaviours;

public class TenantAuthBehaviour<TRequest, TResponse>(
    ICurrentUser currentUser)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : ITenantRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var tenantId = currentUser.GetTenantId();
        if (!tenantId.HasValue || tenantId.Value == Guid.Empty)
        {
            throw new UnauthorizedAccessException();
        }

        return await next();
    }
}