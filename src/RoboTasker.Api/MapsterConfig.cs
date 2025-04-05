using Mapster;
using RoboTasker.Api.Models.Tasks;
using RoboTasker.Application.Common.Models;
using RoboTasker.Application.Robots.Tasks.CreateTask;
using RoboTasker.Application.Robots.Tasks.UpdateTask;

namespace RoboTasker.Api;

public class MapsterConfig
{
    public static void CreateConfig()
    {
        TypeAdapterConfig<CreateTaskRequest, CreateTaskCommand>
            .NewConfig()
            .Ignore(c => c.Files);
        
        TypeAdapterConfig<UpdateTaskRequest, UpdateTaskCommand>
            .NewConfig()
            .Map(dest => dest.DeletedFiles, src => 
                src.DeletedFiles != null ? src.DeletedFiles.Names : new List<string>())
            .Map(dest => dest.DeletedRequirements, src =>
                src.DeletedRequirements != null ? src.DeletedRequirements.Ids : new List<Guid>())
            .Map(dest => dest.DeletedData, src =>
                src.DeletedData != null ? src.DeletedData.Ids : new List<Guid>())
            .Ignore(c => c.Files);
    }
}