namespace RoboTasker.Api.Models.Tenants;

public class UpdateRoleRequest
{
    public string? Name { get; set; }
    public IList<Guid>? DeletePermissions { get; set; }
    public IList<Guid>? Permissions { get; set; }
}