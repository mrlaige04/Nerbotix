using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Application.Robots.Categories;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Robots.Communications;

namespace Nerbotix.Application.Robots.Robots.GetRobotById;

public class GetRobotByIdHandler(
    ITenantRepository<Robot> robotRepository) : IQueryHandler<GetRobotByIdQuery, RobotResponse>
{
    public async Task<ErrorOr<RobotResponse>> Handle(GetRobotByIdQuery request, CancellationToken cancellationToken)
    {
        var robot = await robotRepository.GetAsync(
            r => r.Id == request.Id,
            q => q
                .Include(r => r.Properties)
                .ThenInclude(p => p.Property)
                .Include(r => r.CustomProperties)
                .Include(r => r.Category)
                .Include(r => r.Capabilities)
                .ThenInclude(c => c.Capability)
                .Include(r => r.Communication),
            cancellationToken);
        
        if (robot == null)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }

        var response = new RobotResponse
        {
            Id = robot.Id,
            Name = robot.Name,
            TenantId = robot.TenantId,
            Category = new CategoryBaseResponse
            {
                Id = robot.CategoryId,
                Name = robot.Category.Name,
                TenantId = robot.Category.TenantId,
            },
            Properties = robot.Properties
                .Select(p => new RobotPropertyResponse
                {
                    Id = p.Id,
                    PropertyId = p.PropertyId,
                    Name = p.Property.Name,
                    Value = p.Value.ToString()!,
                    Type = p.Property.Type,
                }).ToList(),
            CustomProperties = robot.CustomProperties
                .Select(cp => new RobotCustomPropertyResponse()
                {
                    Id = cp.Id,
                    Name = cp.Name,
                    Value = cp.Value.ToString()!,
                    TenantId = cp.TenantId,
                    CreatedAt = cp.CreatedAt,
                    UpdatedAt = cp.UpdatedAt,
                }).ToList(),
            Capabilities = robot.Capabilities
                .Select(c => new RobotCapabilityResponse
                {
                    Id = c.CapabilityId,
                    GroupId = c.Capability.GroupId,
                }).ToList(),
            CreatedAt = robot.CreatedAt,
            UpdatedAt = robot.UpdatedAt,
        };

        response.Communication = new RobotCommunicationResponse();
        if (robot.Communication.Type == RobotCommunicationType.Http)
        {
            var httpCommunication = robot.Communication as HttpCommunication;
            
            response.Communication.CommunicationType = RobotCommunicationType.Http;
            response.Communication.ApiEndpoint = httpCommunication!.ApiEndpoint;
            response.Communication.HttpMethod = httpCommunication!.HttpMethod;
            response.Communication.Headers = httpCommunication!.Headers;
        } else if (robot.Communication.Type == RobotCommunicationType.Mqtt)
        {
            var mqttCommunication = robot.Communication as MqttCommunication;
            
            response.Communication.CommunicationType = RobotCommunicationType.Mqtt;
            response.Communication.MqttBrokerAddress = mqttCommunication!.MqttBrokerAddress;
            response.Communication.MqttBrokerUsername = mqttCommunication!.MqttBrokerUsername;
            response.Communication.MqttBrokerPassword = mqttCommunication!.MqttBrokerPassword;
            response.Communication.MqttTopic = mqttCommunication!.MqttTopic;
        }
        
        /*var robot = await robotRepository.GetWithSelectorAsync(
            r => new RobotResponse
            {
                Id = r.Id,
                Name = r.Name,
                TenantId = r.TenantId,
                Category = new CategoryBaseResponse
                {
                    Id = r.CategoryId,
                    Name = r.Category.Name,
                    TenantId = r.Category.TenantId,
                },
                Properties = r.Properties
                    .Select(p => new RobotPropertyResponse
                    {
                        Id = p.Id,
                        PropertyId = p.PropertyId,
                        Name = p.Property.Name,
                        Value = p.Value.ToString()!,
                        Type = p.Property.Type,
                    }).ToList(),
                CustomProperties = r.CustomProperties
                    .Select(cp => new RobotCustomPropertyResponse()
                    {
                        Id = cp.Id,
                        Name = cp.Name,
                        Value = cp.Value.ToString()!,
                        TenantId = cp.TenantId,
                        CreatedAt = cp.CreatedAt,
                        UpdatedAt = cp.UpdatedAt,
                    }).ToList(),
                Capabilities = r.Capabilities
                    .Select(c => new RobotCapabilityResponse
                    {
                        Id = c.CapabilityId,
                        GroupId = c.Capability.GroupId,
                    }).ToList(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
            },
            r => r.Id == request.Id,
            q => q
                .Include(r => r.Properties)
                    .ThenInclude(p => p.Property)
                .Include(r => r.CustomProperties)
                .Include(r => r.Category)
                .Include(r => r.Capabilities)
                    .ThenInclude(c => c.Capability)
                .Include(r => r.Communication),
            cancellationToken);

        if (robot == null)
        {
            return Error.NotFound(RobotErrors.NotFound, RobotErrors.NotFoundDescription);
        }*/

        return response;
    }
}