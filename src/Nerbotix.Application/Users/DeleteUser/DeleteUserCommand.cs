using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Users.DeleteUser;

public record DeleteUserCommand(Guid Id): ITenantCommand;