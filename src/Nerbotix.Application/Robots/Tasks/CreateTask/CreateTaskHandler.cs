using System.IO.Compression;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nerbotix.Application.BackgroundJobs;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Capabilities;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tasks.Data;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Robots.Tasks.CreateTask;

public class CreateTaskHandler(
    ICurrentUser currentUser,
    IJobsService jobsService,
    IConfiguration configuration,
    IBaseRepository<Tenant> tenantRepository,
    ITenantRepository<RobotCategory> categoryRepository,
    ITenantRepository<Capability> capabilityRepository,
    ITenantRepository<RobotTask> taskRepository) : ICommandHandler<CreateTaskCommand, TaskBaseResponse>
{
    public async Task<ErrorOr<TaskBaseResponse>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var tenantId = currentUser.GetTenantId();
        if (!await tenantRepository.ExistsAsync(t => t.Id == tenantId, cancellationToken: cancellationToken))
        {
            return Error.NotFound(TenantErrors.NotFound, TenantErrors.NotFoundDescription);
        }

        var task = new RobotTask
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            EstimatedDuration = request.EstimatedDuration,
            Priority = request.Priority,
            Complexity = request.Complexity
        };
        
        var category = await categoryRepository.GetAsync(
            c => c.Id == request.CategoryId,
            cancellationToken: cancellationToken);

        if (category == null)
        {
            return Error.NotFound(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
        }
        
        task.Category = category;
        
        foreach (var requirement in request.Requirements ?? [])
        {
            var capability = await capabilityRepository.GetAsync(
                c => c.Id == requirement.CapabilityId, 
                cancellationToken: cancellationToken);

            if (capability == null)
            {
                continue;
            }
            
            var newRequirement = new RobotTaskRequirement
            {
                CapabilityId = capability.Id,
                RequiredLevel = requirement.Level,
            };
                
            task.Requirements.Add(newRequirement);
        }
        
        foreach (var data in request.Data ?? [])
        {
            var newData = new RobotTaskData
            {
                Key = data.Key,
                Type = data.Type,
                Value = data.Value?.ToString()!,
            };
            
            task.TaskData.Add(newData);
        }

        if (request.Files is { Count: > 0 })
        {
            task.Archive = await UploadFiles(task.Id, request.Files);
        }

        var createdTask = await taskRepository.AddAsync(task, cancellationToken);

        var jobId = jobsService.EnqueueTask(createdTask.Id);
        
        return new TaskBaseResponse
        {
            Id = createdTask.Id,
            TenantId = createdTask.TenantId,
        };
    }

    private async Task<RobotTaskFiles> UploadFiles(Guid taskId, IFormFileCollection files)
    {
        var root = configuration["Storage:Root"]!;
        var userId = currentUser.GetUserId();
        var directory = Path.Combine(root, userId!.Value.ToString(), "TaskData");
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        var path = Path.Combine(directory, $"{taskId}.zip");
        await using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write); 
        using var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true);

        foreach (var file in files)
        {
            var entry = archive.CreateEntry(file.FileName, CompressionLevel.Optimal);
            await using var entryStream = entry.Open();
            await file.CopyToAsync(entryStream);
        }

        return new RobotTaskFiles
        {
            Url = path,
            FileName = path,
            Size = files.Sum(f => f.Length),
        };
    }
}