using System.Security.Claims;
using Nerbotix.Domain.Services;
using Nerbotix.Infrastructure.Authentication;

namespace Nerbotix.Api.Services;

public class CurrentUser(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration) : ICurrentUser
{
    private Guid? _manualTenantId;
    
    public Guid? GetTenantId()
    {
        if (_manualTenantId != null && _manualTenantId != Guid.Empty)
        {
            return _manualTenantId;
        }
        
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

    public string[]? GetRoles()
    {
        var roleHeader = httpContextAccessor.HttpContext?.User.Claims.Where(c => c.Type == ClaimTypes.Role);
        return roleHeader?.Select(r => r.Value).ToArray();
    }

    public void SetTenantId(Guid? tenantId)
    {
        _manualTenantId = tenantId;
    }

    public string? GetEmail()
    {
        var roleClaim = httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
        return roleClaim?.Value;
    }

    public bool IsSuperAdmin()
    {
        var email = GetEmail();
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }
        
        var superAdminEmail = configuration["SuperAdmin:Email"];
        return email.Equals(superAdminEmail, StringComparison.InvariantCultureIgnoreCase);
    }
}