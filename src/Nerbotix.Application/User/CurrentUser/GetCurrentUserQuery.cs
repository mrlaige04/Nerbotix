using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.User.CurrentUser;

public record GetCurrentUserQuery : ITenantQuery<CurrentUserResponse>;