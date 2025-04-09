using Microsoft.AspNetCore.Http;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Tasks;
using Nerbotix.Domain.Tasks.Data;

namespace Nerbotix.Application.Robots.Tasks.CreateTask;

public class CreateTaskCommand : ITenantCommand<TaskBaseResponse>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public TimeSpan EstimatedDuration { get; set; }
    public int Priority { get; set; }
    public double Complexity { get; set; }

    public Guid CategoryId { get; set; } 

    public IList<CreateTaskRequirementCommand>? Requirements { get; set; }
    
    public IList<CreateTaskDataCommand>? Data { get; set; }
    
    public IFormFileCollection? Files { get; set; }
}

public record CategoryId(Guid Id);

public class CreateTaskRequirementCommand
{
    public Guid CapabilityId { get; set; }
    public RobotTaskRequirementLevel Level { get; set; }
    
    public Guid? ExistingId { get; set; }
}

public class CreateTaskDataCommand
{
    public string Key { get; set; } = null!;
    public RobotTaskDataType Type { get; set; }
    
    public object? Value { get; set; }
    
    public Guid? ExistingId { get; set; }
}