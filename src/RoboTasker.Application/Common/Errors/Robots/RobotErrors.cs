namespace RoboTasker.Application.Common.Errors.Robots;

public static class RobotErrors
{
    public const string NotFound = "Robot.NotFound";
    public const string NotFoundDescription = "Robot not found.";
    
    public const string Conflict = "Robot.Conflict";
    public const string ConflictDescription = "Robot with this name already exists.";
    
    public const string NotAllPropertiesProvided = "Robot.NotAllPropertiesProvided";
    public const string NotAllPropertiesProvidedDescription = "Not all properties from category were provided.";
    
    public const string CategoryPropertiesMismatch = "Robot.CategoryPropertiesMismatch";
    public const string CategoryPropertiesMismatchDescription = "Category properties mismatch.";
}