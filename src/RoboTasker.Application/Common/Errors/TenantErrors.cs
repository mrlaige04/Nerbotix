namespace RoboTasker.Application.Common.Errors;

public static class TenantErrors
{
    public const string NotFound = "Tenant.NotFound";
    public const string NotFoundDescription = "Tenant not found.";
    
    public const string Conflict = "Tenant.Conflict";
    public const string ConflictDescription = "Tenant with the same name or email already exists.";
    
    public const string DeleteSuperFailed = "Tenant.DeleteSuperFailed";
    public const string DeleteSuperFailedDescription = "You cannot delete the super tenant.";
}