namespace Nerbotix.Api.Models.Users;

public class UpdateUserRequest
{
    public string? Username { get; set; }
    public IList<Guid>? DeleteRoles { get; set; }
    public IList<Guid>? Roles { get; set; }
}