using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Users.GetUsers;

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
                EmailVerified = u.EmailConfirmed
            }, cancellationToken: cancellationToken);

        return users;
    }
}