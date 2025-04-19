using Nerbotix.Application.Robots.Categories.UpdateCategory;

namespace Nerbotix.Api.Models.Categories;

public class UpdateCategoryRequest
{
    public string? Name { get; set; }
    public IList<Guid>? DeletedProperties { get; set; }
    public IList<UpdateCategoryPropertyCommand>? NewProperties { get; set; }
}