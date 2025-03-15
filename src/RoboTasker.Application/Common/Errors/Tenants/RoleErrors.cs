namespace RoboTasker.Application.Common.Errors.Tenants;

public static class RoleErrors
{
    public const string NotFound = "Role.NotFound";
    public const string NotFoundDescription = "Role not found.";
    
    public const string Conflict = "Role.Conflict";
    public const string ConflictDescription = "Role with this name already exists.";
    
    public const string CreationFailed = "Role.CreationFailed";
    public const string CreationFailedDescription = "Failed to create the role.";
    
    public const string DeletingFailed = "Role.DeletingFailed";
    public const string DeletingFailedDescription = "Failed to delete the role.";
}