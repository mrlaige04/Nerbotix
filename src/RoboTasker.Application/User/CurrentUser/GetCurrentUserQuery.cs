using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.User.CurrentUser;

public record GetCurrentUserQuery : ITenantQuery<CurrentUserResponse>;