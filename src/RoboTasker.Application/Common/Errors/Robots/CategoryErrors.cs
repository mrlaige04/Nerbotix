namespace RoboTasker.Application.Common.Errors.Robots;

public static class CategoryErrors
{
    public const string NotFound = "Category.NotFound";
    public const string NotFoundDescription = "Category not found.";
    
    public const string Conflict = "Category.Conflict";
    public const string ConflictDescription = "Category with this name already exists.";
}