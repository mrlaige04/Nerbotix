using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Capabilities;
using Nerbotix.Domain.Repositories.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities.DeleteCapability;

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