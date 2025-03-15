using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Roles.Permissions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;
using RoboTasker.Infrastructure.Authentication.Services;

namespace RoboTasker.Infrastructure.Authentication.Login;

public class LoginHandler(
    UserManager<User> userManager, TokenService tokenService,
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
        
        var token = tokenService.GenerateToken(user);
        token.RefreshToken = tokenService.GenerateRefreshToken();
        
        user.RefreshToken = token.RefreshToken;
        user.RefreshTokenExpiresAt = DateTimeOffset.UtcNow.AddDays(2); // TODO: Move in settings
        
        await userManager.UpdateAsync(user);

        var userFromRepo = await userRepository.GetAsync(
            u => u.Id == user.Id,
            q => q
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
        
        var permissions = userFromRepo.Roles
            .SelectMany(r => r.Role.Permissions)
            .Select(rp => rp.Permission)
            .ToList();
        
        var currentUser = new CurrentUserResponse
        {
            Id = user.Id,
            TenantId = user.TenantId,
            Email = user.Email!,
            Permissions = permissions.Select(p => new PermissionBaseResponse
            {
                Id = p.Id,
                Name = p.Name,
                GroupName = p.Group.Name
            }).ToList()
        };

        return new LoginResponse
        {
            Token = token,
            User = currentUser
        };
    }
}