using System.IO.Compression;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors.Robots;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tasks.Data;

namespace RoboTasker.Application.Robots.Tasks.GetTaskById;

public class GetTaskByIdHandler(
    ITenantRepository<RobotTask> taskRepository) : IQueryHandler<GetTaskByIdQuery, TaskResponse>
{
    public async Task<ErrorOr<TaskResponse>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await taskRepository.GetWithSelectorAsync(
            t => new TaskResponse
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                TenantId = t.TenantId,
                CategoryId = t.CategoryId,
                ArchivePath = t.Archive != null ? t.Archive.Url : null,
                Requirements = t.Requirements
                    .Select(r => new TaskRequirementResponse
                    {
                        Id = r.Id,
                        CapabilityId = r.CapabilityId,
                        Level = r.RequiredLevel
                    }).ToList(),
                AssignedRobotId = t.AssignedRobotId,
                Complexity = t.Complexity,
                Priority = t.Priority,
                Status = t.Status,
                EstimatedDuration = t.EstimatedDuration,
                CompletedAt = t.CompletedAt,
                Data = t.TaskData.Select(d => new TaskDataResponse
                {
                    Key = d.Key,
                    Type = d.Type,
                    Id = d.Id,
                    Value = d.Value
                }).ToList()
            },
            r => r.Id == request.Id,
            q => q
                .Include(r => r.Requirements)
                .Include(r => r.TaskData)
                .Include(r => r.Archive),
            cancellationToken);
        
        if (task == null)
        {
            return Error.NotFound(TaskErrors.NotFound, TaskErrors.NotFoundDescription);
        }

        if (!string.IsNullOrEmpty(task.ArchivePath))
        {
            task.Files = GetTaskFiles(task.ArchivePath);
        }

        return task;
    }

    private IList<TaskFileResponse> GetTaskFiles(string archivePath)
    {
        if (!File.Exists(archivePath))
        {
            return [];
        }

        using var archive = ZipFile.OpenRead(archivePath);

        return archive.Entries
            .Select(entry => new TaskFileResponse
            {
                FileName = entry.FullName, 
                ContentType = Path.GetExtension(entry.FullName), 
                Size = entry.Length,
            }).ToList();
    }
}