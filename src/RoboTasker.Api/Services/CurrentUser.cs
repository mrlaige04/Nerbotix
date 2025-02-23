using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using RoboTasker.Domain.Services;
using RoboTasker.Infrastructure.Authentication;

namespace RoboTasker.Api.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public Guid? GetTenantId()
    {
        var tenantHeader = httpContextAccessor.HttpContext?.User
            .FindFirstValue(CustomClaims.TenantId);
        
        return Guid.TryParse(tenantHeader, out var tenant) ? tenant : null;
    }

    public Guid? GetUserId()
    {
        var userHeader = httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.NameIdentifier);
        
        return Guid.TryParse(userHeader, out var userId) ? userId : null;
    }
}