using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Roles.Roles;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;

namespace Nerbotix.Application.Users.GetUsers;

public class GetUsersHandler(
    ITenantRepository<Domain.Tenants.User> userRepository) : IQueryHandler<GetUsersQuery, PaginatedList<UserBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<UserBaseResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            u => new UserBaseResponse
            {
                Id = u.Id,
                Email = u.Email!,
                Username = u.UserName ?? u.Email!,
                EmailVerified = u.EmailConfirmed,
                Roles = u.Roles.Select(ur => new RoleBaseResponse
                {
                    Id = ur.RoleId,
                    Name = ur.Role.Name!
                }).ToList()
            }, 
            include: q => q
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role),
            cancellationToken: cancellationToken);

        return users;
    }
}