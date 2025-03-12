using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RoboTasker.Application.Robots.Tasks.CreateTask;
using RoboTasker.Domain.Tasks.Data;

namespace RoboTasker.Api.Models.Tasks;

public class CreateTaskRequest
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    
    public TimeSpan EstimatedDuration { get; set; }
    public int Priority { get; set; }
    public double Complexity { get; set; }
    
    [FromForm(Name = "properties")]
    public string? PropertiesJson { get; set; }
    
    [FromForm(Name = "requirements")]
    public string? RequirementsJson { get; set; } 
    
    [FromForm(Name = "data")]
    public string? DataJson { get; set; } 

    [JsonIgnore]
    public IList<CreateTaskPropertyCommand>? Properties => JsonConvert.DeserializeObject<IList<CreateTaskPropertyCommand>>(PropertiesJson ?? "[]");

    [JsonIgnore]
    public IList<CreateTaskRequirementCommand>? Requirements => JsonConvert.DeserializeObject<IList<CreateTaskRequirementCommand>>(RequirementsJson?? "[]");

    [JsonIgnore]
    public IList<CreateTaskDataRequest>? Data => JsonConvert.DeserializeObject<IList<CreateTaskDataRequest>>(DataJson?? "[]");
    
    [FromForm]
    public IFormFileCollection? Files { get; set; }
}

public class CreateTaskDataRequest
{
    public string Key { get; set; } = null!;
    public RobotTaskDataType Type { get; set; }
    
    public object? Value { get; set; } 
}