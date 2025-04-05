using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Application.Robots.Categories;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Robots.Robots.CreateRobot;

public class CreateRobotHandler(
    ICurrentUser currentUser,
    IBaseRepository<Tenant> tenantRepository,
    ITenantRepository<Robot> robotRepository,
    ITenantRepository<RobotCategory> categoryRepository,
    ITenantRepository<Capability> capabilityRepository) : ICommandHandler<CreateRobotCommand, RobotBaseResponse>
{
    public async Task<ErrorOr<RobotBaseResponse>> Handle(CreateRobotCommand request, CancellationToken cancellationToken)
    {
        var tenantId = currentUser.GetTenantId();
        if (!await tenantRepository.ExistsAsync(t => t.Id == tenantId, cancellationToken: cancellationToken))
        {
            return Error.NotFound(TenantErrors.NotFound, TenantErrors.NotFoundDescription);
        }
        
        if (await robotRepository.ExistsAsync(c => 
                EF.Functions.Like(c.Name, request.Name), cancellationToken: cancellationToken))
        {
            return Error.Conflict(RobotErrors.Conflict, RobotErrors.ConflictDescription);   
        }
        
        var category = await categoryRepository.GetAsync(
            c => c.Id == request.CategoryId,
            q => q.Include(c => c.Properties),
            cancellationToken: cancellationToken);

        if (category == null)
        {
            return Error.Failure(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
        }
        
        var allCategoryPropertiesProvided = category.Properties
            .Select(c => c.Id)
            .Any(c => request.Properties.Any(p => p.PropertyId == c));

        if (!allCategoryPropertiesProvided)
        {
            return Error.Failure(RobotErrors.NotAllPropertiesProvided, RobotErrors.NotAllPropertiesProvidedDescription);
        }

        if (category.Properties.Count != request.Properties.Count)
        {
            return Error.Failure(RobotErrors.CategoryPropertiesMismatch, RobotErrors.CategoryPropertiesMismatchDescription);
        }

        var robot = new Robot
        {
            Name = request.Name,
            CategoryId = request.CategoryId,
            Category = category,
            Location = RobotLocation.Empty
        };

        foreach (var prop in request.Properties)
        {
            var property = category.Properties.First(p => p.Id == prop.PropertyId);
            var propValue = new RobotPropertyValue
            {
                Property = property,
                Value = prop.Value.ToString()!
            };
            
            robot.Properties.Add(propValue);
        }

        foreach (var customProperty in request.CustomProperties ?? [])
        {
            var customProp = new RobotCustomProperty
            {
                Name = customProperty.Name,
                Value = customProperty.Value.ToString()!
            };
            
            robot.CustomProperties.Add(customProp);
        }
        
        foreach (var capability in request.Capabilities ?? [])
        {
            var foundCapability = await capabilityRepository.GetAsync(
                c => c.Id == capability.Id && c.GroupId == capability.GroupId,
                cancellationToken: cancellationToken);
            
            if (foundCapability == null) continue;

            var robotCapability = new RobotCapability
            {
                CapabilityId = foundCapability.Id,
            };
            
            robot.Capabilities.Add(robotCapability);
        }
        
        var createdRobot = await robotRepository.AddAsync(robot, cancellationToken);

        return new RobotBaseResponse
        {
            Id = createdRobot.Id,
            Name = createdRobot.Name,
            TenantId = createdRobot.TenantId,
            Category = new CategoryBaseResponse
            {
                Id = createdRobot.CategoryId,
                Name = createdRobot.Category.Name,
                TenantId = createdRobot.Category.TenantId
            },
            CreatedAt = createdRobot.CreatedAt,
            UpdatedAt = createdRobot.UpdatedAt,
        };
    }
}