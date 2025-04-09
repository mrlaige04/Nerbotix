using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Application.Roles.Roles;
using Nerbotix.Domain.Repositories.Abstractions;

namespace Nerbotix.Application.Users.GetUserById;

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