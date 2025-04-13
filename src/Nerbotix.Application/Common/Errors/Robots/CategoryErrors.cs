namespace Nerbotix.Application.Common.Errors.Robots;

public static class CategoryErrors
{
    public const string NotFound = "Category.NotFound";
    public const string NotFoundDescription = "Category not found.";
    
    public const string Conflict = "Category.Conflict";
    public const string ConflictDescription = "Category with this name already exists.";
    
    public const string CannotDeleteLinkedEntities = "Category.DeleteFailed";
    public const string CannotDeleteRobotsDescription = "You cannot delete category since there are robots within it.";
    public const string CannotDeleteTasksDescription = "You cannot delete category since there are tasks within it.";
}