using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Application.Roles.Roles;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Users.UpdateUser;

public class UpdateUserHandler(
    ITenantRepository<Domain.Tenants.User> userRepository) : ICommandHandler<UpdateUserCommand, UserBaseResponse>
{
    public async Task<ErrorOr<UserBaseResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(
            u => u.Id == request.Id,
            q => q
                .Include(u => u.Roles),
            cancellationToken);

        if (user == null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }

        if (!string.IsNullOrEmpty(request.Username))
        {
            user.UserName = request.Username;
        }
        
        foreach (var delRole in request.DeleteRoles ?? [])
        {
            var role = user.Roles.FirstOrDefault(role => role.RoleId == delRole);
            if (role != null)
            {
                user.Roles.Remove(role);
            }
        }
        
        foreach (var addRole in request.Roles ?? [])
        {
            var existingRole = user.Roles.FirstOrDefault(role => role.RoleId == addRole);
            if (existingRole != null)
            {
                continue;
            }

            user.Roles.Add(new UserRole
            {
                RoleId = addRole,
            });
        }
        
        var updatedUser = await userRepository.UpdateAsync(user, cancellationToken);

        return new UserBaseResponse
        {
            Id = updatedUser.Id,
            Username = updatedUser.UserName!,
            Email = updatedUser.Email!,
            Roles = user.Roles.Select(ur => new RoleBaseResponse
            {
                Id = ur.RoleId,
            }).ToList()
        };
    }
}