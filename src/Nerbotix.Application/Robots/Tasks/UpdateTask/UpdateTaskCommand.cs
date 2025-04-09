using Microsoft.AspNetCore.Http;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Robots.Tasks.CreateTask;

namespace Nerbotix.Application.Robots.Tasks.UpdateTask;

public class UpdateTaskCommand : ITenantCommand<TaskBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Priority { get; set; }
    public double? Complexity { get; set; }
    public TimeSpan? EstimatedDuration { get; set; }
    
    public Guid? CategoryId { get; set; }
    
    public IList<CreateTaskRequirementCommand>? Requirements { get; set; }
    public IList<CreateTaskDataCommand>? Data { get; set; }
    public IFormFileCollection? Files { get; set; }
    
    public IList<Guid>? DeletedRequirements { get; set; }
    public IList<Guid>? DeletedData { get; set; }
    public IList<string>? DeletedFiles { get; set; }
}