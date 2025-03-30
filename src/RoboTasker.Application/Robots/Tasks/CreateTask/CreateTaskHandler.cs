using System.IO.Compression;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using RoboTasker.Application.BackgroundJobs;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Capabilities;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Robots;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tasks.Data;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Robots.Tasks.CreateTask;

public class CreateTaskHandler(
    ICurrentUser currentUser,
    IJobsService jobsService,
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
            task.Archive = await UploadFiles(request.Files);
        }

        var createdTask = await taskRepository.AddAsync(task, cancellationToken);

        var jobId = jobsService.EnqueueTask(createdTask.Id);
        
        return new TaskBaseResponse
        {
            Id = createdTask.Id,
            TenantId = createdTask.TenantId,
        };
    }

    private async Task<RobotTaskFiles> UploadFiles(IFormFileCollection files)
    {
        var userId = currentUser.GetUserId();
        var directory = Path.Combine(Path.GetTempPath(), "Archives", userId!.Value.ToString());
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        var id = Guid.NewGuid();
        var path = Path.Combine(directory, $"{id}.zip");
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