using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Capabilities;
using Nerbotix.Domain.Repositories.Abstractions;

namespace Nerbotix.Application.Robots.Capabilities.GetCapabilityById;

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