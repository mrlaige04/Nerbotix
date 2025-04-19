namespace Nerbotix.Application.Services;

public interface ITaskAssignerService
{
    Task<bool> AssignTask(Guid taskId);
}