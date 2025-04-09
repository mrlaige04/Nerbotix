using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;

namespace Nerbotix.Application.Robots.Robots.DeleteRobot;

public class DeleteRobotHandler(
    ITenantRepository<Robot> robotRepository) : ICommandHandler<DeleteRobotCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteRobotCommand request, CancellationToken cancellationToken)
    {
        var robot = await robotRepository.GetAsync(
            c => c.Id == request.Id,
            cancellationToken: cancellationToken);
        
        if (robot == null)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }
        
        await robotRepository.DeleteAsync(robot, cancellationToken);
        
        return new Success();
    }
}