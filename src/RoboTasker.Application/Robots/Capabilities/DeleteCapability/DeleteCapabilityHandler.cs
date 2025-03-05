using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.DeleteCapability;

public class DeleteCapabilityHandler(
    ITenantRepository<CapabilityGroup> capabilityGroupRepository) : ICommandHandler<DeleteCapabilityCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteCapabilityCommand request, CancellationToken cancellationToken)
    {
        var group = await capabilityGroupRepository.GetAsync(
            c => c.Id == request.Id,
            cancellationToken: cancellationToken);
        
        if (group == null)
        {
            return Error.NotFound(CapabilityErrors.NotFound, CapabilityErrors.NotFoundDescription);
        }
        
        await capabilityGroupRepository.DeleteAsync(group, cancellationToken);
        
        return new Success();
    }
}