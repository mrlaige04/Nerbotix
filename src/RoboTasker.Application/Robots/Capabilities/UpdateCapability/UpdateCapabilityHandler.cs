using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.UpdateCapability;

public class UpdateCapabilityHandler(
    ITenantRepository<CapabilityGroup> capabilityGroupRepository) : ICommandHandler<UpdateCapabilityCommand, CapabilityBaseResponse>
{
    public async Task<ErrorOr<CapabilityBaseResponse>> Handle(UpdateCapabilityCommand request, CancellationToken cancellationToken)
    {
        var capability = await capabilityGroupRepository.GetAsync(
            c => c.Id == request.Id,
            q => q
                .Include(c => c.Capabilities),
            cancellationToken: cancellationToken);
        
        if (capability == null)
        {
            return Error.NotFound(CapabilityErrors.NotFound, CapabilityErrors.NotFoundDescription);
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            capability.Name = request.Name;
        }

        if (!string.IsNullOrEmpty(request.Description))
        {
            capability.Description = request.Description;
        }
        
        foreach (var deletedCapability in request.DeletedCapabilities ?? [])
        {
            var property = capability.Capabilities.FirstOrDefault(p => p.Id == deletedCapability);
            if (property != null)
            {
                capability.Capabilities.Remove(property);
            }
        }
        
        foreach (var newCapabilityItem in request.NewCapabilities ?? [])
        {
            var newCapability = new Capability
            {
                Name = newCapabilityItem.Name,
                Description = newCapabilityItem.Description
            };
            
            capability.Capabilities.Add(newCapability);
        }
        
        var updatedCapability = await capabilityGroupRepository.UpdateAsync(capability, cancellationToken);

        return new CapabilityBaseResponse
        {
            Id = updatedCapability.Id,
            GroupName = updatedCapability.Name,
            TenantId = updatedCapability.TenantId,
            CreatedAt = updatedCapability.CreatedAt,
            UpdatedAt = updatedCapability.UpdatedAt,
        };
    }
}