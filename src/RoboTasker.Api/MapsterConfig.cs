using Mapster;
using RoboTasker.Api.Models.Tasks;
using RoboTasker.Application.Robots.Tasks.CreateTask;

namespace RoboTasker.Api;

public class MapsterConfig
{
    public static void CreateConfig()
    {
        TypeAdapterConfig<CreateTaskRequest, CreateTaskCommand>
            .NewConfig()
            .Ignore(c => c.Files);
    }
}