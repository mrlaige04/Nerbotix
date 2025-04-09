using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : ITenantQuery<UserResponse>;