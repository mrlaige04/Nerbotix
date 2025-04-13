using System.IO.Compression;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Capabilities;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Robots;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Application.Robots.Tasks.UpdateTask;

public class UpdateTaskHandler(
    ICurrentUser currentUser,
    IConfiguration configuration,
    ITenantRepository<RobotTask> taskRepository,
    ITenantRepository<RobotCategory> robotCategoryRepository,
    ITenantRepository<Capability> capabilityRepository) : ICommandHandler<UpdateTaskCommand, TaskBaseResponse>
{
    public async Task<ErrorOr<TaskBaseResponse>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetAsync(
            c => c.Id == request.Id,
            q => q
                .Include(t => t.Requirements)
                .Include(t => t.TaskData)
                .Include(t => t.Archive),
            cancellationToken: cancellationToken);
        
        if (task == null)
        {
            return Error.NotFound(TaskErrors.NotFound, TaskErrors.NotFoundDescription);
        }

        if (request.CategoryId.HasValue && request.CategoryId != task.CategoryId)
        {
            var category = await robotCategoryRepository.GetAsync(
                c => c.Id == request.CategoryId,
                cancellationToken: cancellationToken);

            if (category == null)
            {
                return Error.NotFound(CategoryErrors.NotFound, CategoryErrors.NotFoundDescription);
            }
            
            task.Category = category;
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            task.Name = request.Name;
        }

        task.Description = request.Description;

        if (request.Priority.HasValue)
        {
            task.Priority = request.Priority.Value;
        }

        if (request.Complexity.HasValue)
        {
            task.Complexity = request.Complexity.Value;
        }

        if (request.EstimatedDuration.HasValue)
        {
            task.EstimatedDuration = request.EstimatedDuration.Value;
        }
        
        
        foreach (var deleteRequirement in request.DeletedRequirements ?? [])
        {
            var requirement = task.Requirements.FirstOrDefault(p => p.CapabilityId == deleteRequirement);
            if (requirement != null)
            {
                task.Requirements.Remove(requirement);
            }
        }
        
        foreach (var deleteData in request.DeletedData ?? [])
        {
            var data = task.TaskData.FirstOrDefault(p => p.Id == deleteData);
            if (data != null)
            {
                task.TaskData.Remove(data);
            }
        }
        
        foreach (var requirement in request.Requirements ?? [])
        {
            if (requirement.ExistingId.HasValue)
            {
                var existingReq = task.Requirements.FirstOrDefault(
                    p => p.Id == requirement.ExistingId.Value && p.RequiredLevel != requirement.Level);
                if (existingReq != null)
                {
                    existingReq.RequiredLevel = requirement.Level;
                }
            }
            else
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
        }
        
        foreach (var data in request.Data ?? [])
        {
            if (data.ExistingId.HasValue)
            {
                var existingData = task.TaskData.FirstOrDefault(
                    p => p.Id == data.ExistingId.Value && (p.Type != data.Type || p.Value != data.Value?.ToString()));
                if (existingData != null)
                {
                    existingData.Value = data.Value?.ToString() ?? string.Empty;
                    existingData.Type = data.Type;
                }
            }
            else
            {
                var newData = new RobotTaskData
                {
                    Key = data.Key,
                    Type = data.Type,
                    Value = data.Value?.ToString()!,
                };

                task.TaskData.Add(newData);
            }
        }

        if (request.DeletedFiles != null || request.Files != null)
        {
            if (task.Archive == null)
            {
                task.Archive = await CreateArchiveAsync(task.Id, request.Files);
            }
            else
            {
                task.Archive = await UpdateArchiveAsync(task.Archive.Url, request.DeletedFiles?.ToArray(), request.Files);
            }
        }
        
        var updatedTask = await taskRepository.UpdateAsync(task, cancellationToken);
        
        return new TaskBaseResponse
        {
            Id = updatedTask.Id,
            TenantId = updatedTask.TenantId,
        };
    }

    private async Task<RobotTaskFiles?> CreateArchiveAsync(Guid taskId, IFormFileCollection? files)
    {
        if (files == null || files.Count == 0)
        {
            return null;
        }
        
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

    private async Task<RobotTaskFiles?> UpdateArchiveAsync(string path, string[]? deleteFiles, IFormFileCollection? createFiles)
    {
        var root = configuration["Storage:Root"]!;
        var userId = currentUser.GetUserId();
        var directory = Path.Combine(root, "TaskData", userId!.Value.ToString());

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(path))
        {
            await File.Create(path).DisposeAsync();
        }

        var tempDir = Path.Combine(root, userId.Value.ToString(), "Temp", Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            ZipFile.ExtractToDirectory(path, tempDir, true);

            if (deleteFiles != null)
            {
                foreach (var deleteFile in deleteFiles)
                {
                    var filePath = Path.Combine(tempDir, deleteFile);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }

            if (createFiles != null)
            {
                foreach (var file in createFiles)
                {
                    var newFilePath = Path.Combine(tempDir, file.FileName);
                    await using var stream = new FileStream(newFilePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            ZipFile.CreateFromDirectory(tempDir, path, CompressionLevel.Optimal, false);

            Directory.Delete(tempDir, true);

            return new RobotTaskFiles
            {
                Url = path,
                FileName = Path.GetFileName(path),
                Size = new FileInfo(path).Length,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error updating archive: {e.Message}");
            return null;
        }
    }
}