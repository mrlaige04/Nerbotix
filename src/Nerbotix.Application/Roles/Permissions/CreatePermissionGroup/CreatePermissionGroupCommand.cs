using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Roles.Permissions.CreatePermissionGroup;

public class CreatePermissionGroupCommand : ITenantCommand<PermissionGroupBaseResponse>
{
    public string Name { get; set; } = null!;
    public IList<CreatePermissionGroupItem> Permissions { get; set; } = [];
}

public class CreatePermissionGroupItem
{
    public string Name { get; set; } = null!;
    public Guid? ExistingId { get; set; }
}