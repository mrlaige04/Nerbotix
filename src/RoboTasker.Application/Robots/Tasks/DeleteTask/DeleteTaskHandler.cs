using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tasks;

namespace RoboTasker.Application.Robots.Tasks.DeleteTask;

public class DeleteTaskHandler(
    ITenantRepository<RobotTask> taskRepository) : ICommandHandler<DeleteTaskCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetAsync(
            c => c.Id == request.Id,
            q => q.Include(t => t.Archive),
            cancellationToken: cancellationToken);
        
        if (task == null)
        {
            return Error.NotFound(TaskErrors.NotFound, TaskErrors.NotFoundDescription);
        }
        
        await taskRepository.DeleteAsync(task, cancellationToken);

        if (task.Archive != null)
        {
            File.Delete(task.Archive.Url);
        }
        
        return new Success();
    }
}