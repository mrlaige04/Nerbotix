using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Robots.Categories.CreateCategory;

namespace RoboTasker.Application.Robots.Categories.UpdateCategory;

public class UpdateCategoryCommand : ICommand<CategoryBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public IList<Guid>? DeletedProperties { get; set; }
    public IList<CreateCategoryCommandPropertyItem>? NewProperties { get; set; }
}