using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Robots;

namespace RoboTasker.Application.Robots.Categories.CreateCategory;

public class CreateCategoryCommand : ICommand<CategoryBaseResponse>
{
    public string Name { get; set; } = null!;
    public IList<CreateCategoryCommandPropertyItem> Properties { get; set; } = [];
}

public class CreateCategoryCommandPropertyItem
{
    public string Name { get; set; } = null!;
    public RobotPropertyType Type { get; set; }
}