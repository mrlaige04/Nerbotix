using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Robots.Capabilities.CreateCapability;

public class CreateCapabilityHandler(
    ICurrentUser currentUser,
    IBaseRepository<Tenant> tenantRepository,
    ITenantRepository<CapabilityGroup> capabilityGroupRepository) : ICommandHandler<CreateCapabilityCommand, CapabilityBaseResponse>
{
    public async Task<ErrorOr<CapabilityBaseResponse>> Handle(CreateCapabilityCommand request, CancellationToken cancellationToken)
    {
        var tenantId = currentUser.GetTenantId();
        if (!await tenantRepository.ExistsAsync(t => t.Id == tenantId, cancellationToken: cancellationToken))
        {
            return Error.NotFound(TenantErrors.NotFound, TenantErrors.NotFoundDescription);
        }
        
        if (await capabilityGroupRepository.ExistsAsync(c => 
                EF.Functions.Like(c.Name, request.Name), cancellationToken: cancellationToken))
        {
            return Error.Conflict(CapabilityErrors.Conflict, CapabilityErrors.ConflictDescription);   
        }

        var capabilities = request.Capabilities
            .Select(c => new Capability
            {
                Name = c.Name,
                Description = c.Description
            }).ToList();

        var group = new CapabilityGroup
        {
            Name = request.Name,
            Description = request.Description,
            Capabilities = capabilities
        };
        
        var createdGroup = await capabilityGroupRepository.AddAsync(group, cancellationToken);

        return new CapabilityBaseResponse
        {
            Id = createdGroup.Id,
            GroupName = createdGroup.Name,
            Description = createdGroup.Description,
            TenantId = createdGroup.TenantId,
            CapabilitiesCount = capabilities.Count
        };
    }
}