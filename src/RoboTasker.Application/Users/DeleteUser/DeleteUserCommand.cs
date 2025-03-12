using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Users.DeleteUser;

public record DeleteUserCommand(Guid Id): ITenantCommand;