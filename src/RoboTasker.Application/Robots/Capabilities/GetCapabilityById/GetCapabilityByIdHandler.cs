using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;

namespace RoboTasker.Application.Robots.Capabilities.GetCapabilityById;

public class GetCapabilityByIdHandler(
    ITenantRepository<CapabilityGroup> capabilityGroupRepository) : IQueryHandler<GetCapabilityByIdQuery, CapabilityResponse>
{
    public async Task<ErrorOr<CapabilityResponse>> Handle(GetCapabilityByIdQuery request, CancellationToken cancellationToken)
    {
        var capability = await capabilityGroupRepository.GetWithSelectorAsync(
            c => new CapabilityResponse()
            {
                Id = c.Id,
                GroupName = c.Name,
                Description = c.Description,
                TenantId = c.TenantId,
                Capabilities = c.Capabilities
                    .Select(ci => new CapabilityItemResponse
                    {
                        Id = ci.Id,
                        Name = ci.Name,
                        Description = ci.Description
                    }).ToList(),
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
            },
            c => c.Id == request.Id,
            q => q.Include(c => c.Capabilities),
            cancellationToken: cancellationToken);
        
        if (capability == null)
        {
            return Error.NotFound(CapabilityErrors.NotFound, CapabilityErrors.NotFoundDescription);
        }
        
        return capability;
    }
}