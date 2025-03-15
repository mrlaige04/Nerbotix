using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Users.GetUserById;

public record GetUserByIdQuery(Guid Id) : ITenantQuery<UserResponse>;