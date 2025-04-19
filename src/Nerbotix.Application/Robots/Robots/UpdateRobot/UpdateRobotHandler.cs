using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Application.Robots.Categories;
using Nerbotix.Domain.Capabilities;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Robots.Communications;

namespace Nerbotix.Application.Robots.Robots.UpdateRobot;

public class UpdateRobotHandler(
    ITenantRepository<Robot> robotRepository,
    ITenantRepository<RobotCategory> categoryRepository,
    ITenantRepository<Capability> capabilityRepository) : ICommandHandler<UpdateRobotCommand, RobotBaseResponse>
{
    public async Task<ErrorOr<RobotBaseResponse>> Handle(UpdateRobotCommand request, CancellationToken cancellationToken)
    {
        var robot = await robotRepository.GetAsync(
            c => c.Id == request.Id,
            q => q
                .Include(r => r.Properties)
                .Include(r => r.CustomProperties)
                .Include(r => r.Capabilities)
                    .ThenInclude(c => c.Capability)
                .Include(r => r.Communication),
            cancellationToken: cancellationToken);
        
        if (robot == null)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }
        
        if (!string.IsNullOrEmpty(request.Name))
        {
            robot.Name = request.Name;
        }

        var categoryChanged = false;
        RobotCategory? category;
        if (request.CategoryId.HasValue && request.CategoryId.Value != robot.CategoryId)
        {
            category = await categoryRepository.GetAsync(
                c => c.Id == request.CategoryId, 
                cancellationToken: cancellationToken);

            if (category != null)
            {
                categoryChanged = true;
                robot.CategoryId = category.Id;
            }
        }

        category = await categoryRepository.GetAsync(
            c => c.Id == robot.CategoryId,
            q => q.Include(c => c.Properties),
            cancellationToken: cancellationToken);
        
        if (category == null)
        {
            return Error.Failure(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
        }
        
        
        if (categoryChanged)
        {
            robot.Properties.Clear();
            
            foreach (var prop in request.UpdatedProperties ?? [])
            {
                var property = category.Properties.First(p => p.Id == prop.PropertyId);
                var propValue = new RobotPropertyValue
                {
                    Property = property,
                    Value = prop.Value.ToString()!
                };
            
                robot.Properties.Add(propValue);
            }
        }
        else
        {
            foreach (var updatedProperty in request.UpdatedProperties ?? [])
            {
                var existsInCategory = category.Properties.Any(c => c.Id == updatedProperty.PropertyId);
                if (!existsInCategory)
                {
                    return Error.Failure(RobotErrors.CategoryPropertiesMismatch, RobotErrors.CategoryPropertiesMismatchDescription);
                }
            
                var property = robot.Properties.FirstOrDefault(r => r.PropertyId == updatedProperty.PropertyId);
                if (property == null)
                {
                    return Error.Failure(RobotErrors.CategoryPropertiesMismatch, RobotErrors.CategoryPropertiesMismatchDescription);
                }
            
                property.Value = updatedProperty.Value.ToString()!;
            }
        }
        
        
        foreach (var deletedCustomProperty in request.DeletedCustomProperties ?? [])
        {
            var property = robot.CustomProperties.FirstOrDefault(r => r.Id == deletedCustomProperty);
            if (property != null)
            {
                robot.CustomProperties.Remove(property);
            }
        }
        
        foreach (var newCustomProperty in request.NewCustomProperties ?? [])
        {
            var existingProperty = robot.CustomProperties.FirstOrDefault(r => 
                r.Name.Equals(newCustomProperty.Name, StringComparison.InvariantCultureIgnoreCase));

            if (existingProperty != null)
            {
                existingProperty.Value = newCustomProperty.Value.ToString()!;
            }
            else
            {
                var newProperty = new RobotCustomProperty
                {
                    Name = newCustomProperty.Name,
                    Value = newCustomProperty.Value.ToString()!
                };
                robot.CustomProperties.Add(newProperty);
            }
        }
        
        
        foreach (var deletedCapability in request.DeletedCapabilities ?? [])
        {
            var capability = robot.Capabilities.FirstOrDefault(
                c => c.Capability.Id == deletedCapability.Id && c.Capability.GroupId == deletedCapability.GroupId);

            if (capability != null)
            {
                robot.Capabilities.Remove(capability);
            }
        }
        
        
        foreach (var newCapabilityItem in request.NewCapabilities ?? [])
        {
            var capability = await capabilityRepository.GetAsync(
                c => c.GroupId == newCapabilityItem.GroupId && c.Id == newCapabilityItem.Id,
                cancellationToken: cancellationToken);

            if (capability == null)
            {
                continue;
            }
            
            var newCapability = new RobotCapability
            {
                CapabilityId = capability.Id
            };
                
            robot.Capabilities.Add(newCapability);
        }

        if (request.CommunicationType.HasValue)
        {
            robot.Communication = request switch
            {
                { CommunicationType: RobotCommunicationType.Http, HttpCommunication: not null } => new HttpCommunication
                {
                    ApiEndpoint = request.HttpCommunication.Url,
                    HttpMethod = HttpMethod.Parse(request.HttpCommunication.Method).ToString(),
                    Headers = request.HttpCommunication.Headers?.ToDictionary(
                        h => h.Name, h => h.Value) ?? [],
                },
                { CommunicationType: RobotCommunicationType.Mqtt, MqttCommunication: not null } => new MqttCommunication
                {
                    MqttBrokerAddress = request.MqttCommunication.MqttBrokerAddress,
                    MqttBrokerUsername = request.MqttCommunication.MqttBrokerUsername,
                    MqttBrokerPassword = request.MqttCommunication.MqttBrokerPassword,
                    MqttTopic = request.MqttCommunication.MqttTopic,
                },
                _ => robot.Communication
            };
        }

        var updatedRobot = await robotRepository.UpdateAsync(robot, cancellationToken);
        
        return new RobotBaseResponse
        {
            Id = updatedRobot.Id,
            Name = updatedRobot.Name,
            TenantId = updatedRobot.TenantId,
            Category = new CategoryBaseResponse
            {
                Id = updatedRobot.CategoryId,
                Name = updatedRobot.Category.Name,
                TenantId = updatedRobot.Category.TenantId
            },
            CreatedAt = updatedRobot.CreatedAt,
            UpdatedAt = updatedRobot.UpdatedAt,
        };
    }
}