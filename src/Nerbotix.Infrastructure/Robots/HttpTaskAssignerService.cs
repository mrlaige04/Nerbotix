using System.Text;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Robots.Robots.AssignNewTask;
using Nerbotix.Application.Robots.Tasks;
using Nerbotix.Application.Services;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Robots.Communications;
using Nerbotix.Domain.Robots.Enums;
using Nerbotix.Domain.Tasks;
using Newtonsoft.Json;

namespace Nerbotix.Infrastructure.Robots;

public class HttpTaskAssignerService(
    IHttpClientFactory httpClientFactory,
    ITenantRepository<RobotTask> taskRepository,
    ITenantRepository<Robot> robotRepository) : ITaskAssignerService
{
    public async Task<bool> AssignTask(Guid taskId)
    {
        var client = httpClientFactory.CreateClient();
        
        var task = await taskRepository.GetAsync(
            t => t.Id == taskId,
            q => q
                .Include(t => t.AssignedRobot)
                .ThenInclude(r => r.Communication));

        if (task?.AssignedRobot == null)
        {
            return false;
        }
        
        var robot = task.AssignedRobot;
        if (robot.Communication.Type != RobotCommunicationType.Http)
        {
            return false;
        }

        if (robot.Communication is not HttpCommunication httpCommunication)
        {
            return false;
        }
        
        try
        {
            using var request = new HttpRequestMessage(
                HttpMethod.Parse(httpCommunication.HttpMethod), 
                new Uri(httpCommunication.ApiEndpoint));
            
            var body = new AssignNewTaskCommand
            {
                TaskId = task.Id,
                RobotId = robot.Id,
                RobotName = robot.Name,
                Name = task.Name,
                Description = task.Description,
                EstimatedDuration = task.EstimatedDuration,
                Priority = task.Priority,
                Complexity = task.Complexity,
                Data = task.TaskData
                    .Select(t => new TaskDataResponse
                    {
                        Id = t.Id,
                        Type = t.Type,
                        Key = t.Key,
                        Value = t.Value,
                    }).ToList(),
            };
            
            var content = JsonConvert.SerializeObject(body);
            request.Content = new StringContent(content, Encoding.UTF8, "application/json");
            
            using var response = await client.SendAsync(request);
            
            response.EnsureSuccessStatusCode();

            task.Status = RobotTaskStatus.InProgress;
            await taskRepository.UpdateAsync(task);
            
            robot.Status = RobotStatus.Busy;
            await robotRepository.UpdateAsync(robot);
            
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}