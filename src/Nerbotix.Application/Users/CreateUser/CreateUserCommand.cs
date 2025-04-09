using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Users.CreateUser;

public class CreateUserCommand : ITenantCommand
{
    public string Email { get; set; } = null!;
    public string? Username { get; set; }
    public IList<Guid> Roles { get; set; } = [];
}