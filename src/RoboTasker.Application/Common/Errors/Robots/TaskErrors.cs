namespace RoboTasker.Application.Common.Errors.Robots;

public static class TaskErrors
{
    public const string NotFound = "Task.NotFound";
    public const string NotFoundDescription = "Task not found.";
    
    public const string AlreadyAssigned = "Task.AlreadyAssigned";
    public const string AlreadyAssignedDescription = "You cannot assign task since it's already assigned or completed.";
}