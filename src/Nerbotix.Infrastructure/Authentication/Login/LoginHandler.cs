using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Application.Roles.Permissions;
using Nerbotix.Application.Roles.Roles;
using Nerbotix.Application.User;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;
using Nerbotix.Infrastructure.Authentication.Services;

namespace Nerbotix.Infrastructure.Authentication.Login;

public class LoginHandler(
    UserManager<User> userManager, TokenService tokenService,
    ITenantRepository<Role> roleRepository,
    ITenantRepository<User> userRepository) 
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<ErrorOr<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }

        if (!user.EmailConfirmed)
        {
            return Error.Unauthorized(UserErrors.EmailUnverified, UserErrors.EmailUnverifiedDescription);
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Error.Unauthorized(UserErrors.InvalidPassword, UserErrors.InvalidPasswordDescription);
        }
        
        var token = await tokenService.GenerateToken(user);
        token.RefreshToken = tokenService.GenerateRefreshToken();
        
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(2); // TODO: Move in settings
        
        await userManager.UpdateAsync(user);

        var userFromRepo = await userRepository.GetAsync(
            u => u.Id == user.Id,
            q => q
                .Include(u => u.Tenant)
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role)
                .ThenInclude(r => r.Permissions)
                .ThenInclude(ur => ur.Permission)
                .ThenInclude(p => p.Group),
            cancellationToken);

        if (userFromRepo is null)
        {
            throw new InvalidOperationException(UserErrors.NotFound);
        }
        
        var currentUser = new CurrentUserResponse
        {
            Id = user.Id,
            TenantId = user.TenantId,
            TenantName = user.Tenant.Name,
            Email = user.Email!,
            Roles = user.Roles.Select(ur => new RoleBaseResponse
            {
                Id = ur.RoleId,
                Name = ur.Role.Name!,
                IsSystem = ur.Role.IsSystem,
            }).ToList(),
            Permissions = userFromRepo.Roles
                .SelectMany(ur => ur.Role.Permissions)
                .Select(rp => new PermissionBaseResponse
                {
                    Id = rp.PermissionId,
                    Name = rp.Permission.Name,
                    GroupName = rp.Permission.Group.Name,
                    IsSystem = rp.Permission.IsSystem
                }).ToList()
        };
        
        currentUser.Permissions = currentUser.Permissions.DistinctBy(p => p.Name).ToList();

        return new LoginResponse
        {
            Token = token,
            User = currentUser
        };
    }
}