using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Users.GetUsers;

public class GetUsersQuery : ITenantQuery<PaginatedList<UserBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}