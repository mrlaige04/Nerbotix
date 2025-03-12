using Microsoft.AspNetCore.Http;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Tasks;
using RoboTasker.Domain.Tasks.Data;

namespace RoboTasker.Application.Robots.Tasks.CreateTask;

public class CreateTaskCommand : ITenantCommand<TaskBaseResponse>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public TimeSpan EstimatedDuration { get; set; }
    public int Priority { get; set; }
    public double Complexity { get; set; }

    public IList<CreateTaskPropertyCommand>? Properties { get; set; } 
    public IList<CreateTaskRequirementCommand>? Requirements { get; set; }
    
    public IList<CreateTaskDataCommand>? Data { get; set; }
    
    public IFormFileCollection? Files { get; set; }
}

public class CreateTaskPropertyCommand
{
    public string Key { get; set; } = null!;
    public object Value { get; set; } = null!;
    
    public Guid? ExistingId { get; set; }
}

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