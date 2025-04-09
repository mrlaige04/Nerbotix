using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Users.GetUsers;

public class GetUsersQuery : ITenantQuery<PaginatedList<UserBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}