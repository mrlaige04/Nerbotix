using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Api.Attributes;

public class RoleFilter(
    ICurrentUser currentUser,
    ITenantRepository<User> userRepository,
    string role) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = currentUser.GetUserId();
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var rolesFromJwt = currentUser.GetRoles();
        var roles = await userRepository.GetWithSelectorAsync(
            u => u.Roles.Select(r => r.Role),
            u => u.Id == user,
            q => q
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role));
        
        var hasRole = roles?.Any(r => r.Name == role) ?? false;
        var jwtRole = rolesFromJwt?.Any(r => r == role) ?? false;
        var hasAccess = hasRole && jwtRole;
        if (!hasAccess)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}