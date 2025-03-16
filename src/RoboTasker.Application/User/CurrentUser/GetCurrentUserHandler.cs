using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Roles.Permissions;
using RoboTasker.Application.Roles.Roles;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.User.CurrentUser;

public class GetCurrentUserHandler(
    ICurrentUser currentUser,
    ITenantRepository<Role> roleRepository,
    ITenantRepository<Domain.Tenants.User> userRepository) : IQueryHandler<GetCurrentUserQuery, CurrentUserResponse>
{
    public async Task<ErrorOr<CurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.GetUserId();
        if (userId == null || userId == Guid.Empty)
        {
            return Error.Unauthorized(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }
        
        var user = await userRepository.GetWithSelectorAsync(
            u => new CurrentUserResponse
            {
                Id = u.Id,
                TenantId = u.TenantId,
                Email = u.Email!,
                Roles = u.Roles.Select(ur => new RoleBaseResponse
                {
                    Id = ur.RoleId,
                    Name = ur.Role.Name!,
                    IsSystem = ur.Role.IsSystem,
                }).ToList(),
                Permissions = u.Roles.SelectMany(ur => ur.Role.Permissions)
                    .Select(rp => new PermissionBaseResponse
                    {
                        Id = rp.PermissionId,
                        Name = rp.Permission.Name!,
                        IsSystem = rp.Permission.IsSystem,
                        GroupName = rp.Permission.Group.Name
                    }).ToList()
            },
            u => u.Id == userId,
            q => q
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role)
                .ThenInclude(r => r.Permissions)
                .ThenInclude(rp => rp.Permission),
            cancellationToken);
        
        if (user == null)
        {
            return Error.Unauthorized(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }

        user.Permissions = user.Permissions
            .DistinctBy(p => p.Name)
            .ToList();
        
        return user;
    }
}