using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Users.UpdateUser;

public class UpdateUserCommand : ITenantCommand<UserBaseResponse>
{
    public Guid Id { get; set; }
    public string? Username { get; set; }
    public IList<Guid>? DeleteRoles { get; set; }
    public IList<Guid>? Roles { get; set; }
}