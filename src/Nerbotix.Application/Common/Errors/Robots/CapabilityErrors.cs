namespace Nerbotix.Application.Common.Errors.Robots;

public static class CapabilityErrors
{
    public const string NotFound = "Capability.NotFound";
    public const string NotFoundDescription = "Capability not found.";
    
    public const string Conflict = "Capability.Conflict";
    public const string ConflictDescription = "Capability with this name already exists.";
}