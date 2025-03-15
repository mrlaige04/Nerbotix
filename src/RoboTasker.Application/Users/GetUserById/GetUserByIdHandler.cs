using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Roles.Roles;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Users.GetUserById;

public class GetUserByIdHandler(
    ITenantRepository<Domain.Tenants.User> userRepository) : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<ErrorOr<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetWithSelectorAsync(
            u => new UserResponse
            {
                Id = u.Id,
                Email = u.Email!,
                Username = u.UserName!,
                EmailVerified = u.EmailConfirmed,
                Roles = u.Roles.Select(ur => new RoleBaseResponse
                {
                    Id = ur.RoleId,
                    Name = ur.Role.Name!
                }).ToList()
            },
            u => u.Id == request.Id,
            q => q
                .Include(u => u.Roles)
                .ThenInclude(r => r.Role),
            cancellationToken: cancellationToken);

        if (user == null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }

        return user;
    }
}