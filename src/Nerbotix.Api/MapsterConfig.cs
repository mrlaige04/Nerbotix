using Mapster;
using Nerbotix.Api.Models.Tasks;
using Nerbotix.Application.Robots.Tasks.CreateTask;
using Nerbotix.Application.Robots.Tasks.UpdateTask;

namespace Nerbotix.Api;

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