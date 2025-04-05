using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Roles.Permissions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Api.Attributes;

public class PermissionFilter(
    ICurrentUser currentUser,
    ITenantRepository<User> userRepository,
    PermissionCombining combining,
    string[] permissions) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = currentUser.GetUserId();
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var userPermissions = await userRepository.GetWithSelectorAsync(
            u => u.Roles
                .Select(r => r.Role)
                .SelectMany(r => r.Permissions
                    .Select(p => p.Permission)),
            u => u.Id == user) ?? [];

        var hasPermission = combining == PermissionCombining.All ? 
            permissions.All(p => userPermissions.Any(up => up.Name == p)) : 
            permissions.Any(p => userPermissions.Any(u => u.Name == p));
        
        if (!hasPermission)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}