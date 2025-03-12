using ErrorOr;
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
            cancellationToken: cancellationToken);
        
        if (task == null)
        {
            return Error.NotFound(TaskErrors.NotFound, TaskErrors.NotFoundDescription);
        }
        
        await taskRepository.DeleteAsync(task, cancellationToken);
        
        return new Success();
    }
}