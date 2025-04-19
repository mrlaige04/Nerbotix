using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Robots.Categories.CreateCategory;

namespace Nerbotix.Application.Robots.Categories.UpdateCategory;

public class UpdateCategoryCommand : ITenantCommand<CategoryBaseResponse>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public IList<Guid>? DeletedProperties { get; set; }
    public IList<UpdateCategoryPropertyCommand>? NewProperties { get; set; }
}

public class UpdateCategoryPropertyCommand : CreateCategoryCommandPropertyItem
{
    public Guid? ExistingId { get; set; }
}