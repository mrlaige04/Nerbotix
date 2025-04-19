using System.IO.Compression;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors.Robots;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Application.Robots.Tasks.GetTaskById;

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
                AssignedRobotId = t.AssignedRobotId ?? t.CompletedRobotId,
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
                }).ToList(),
                Logs = t.Logs
                    .OrderBy(l => l.Timestamp)
                    .Select(l => new LogResponse
                    {
                        LogLevel = l.Level,
                        Message = l.Message,
                        Timestamp = l.Timestamp.DateTime,
                    }).ToList(),
            },
            r => r.Id == request.Id,
            q => q
                .Include(r => r.Requirements)
                .Include(r => r.TaskData)
                .Include(r => r.Archive)
                .Include(r => r.Logs),
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