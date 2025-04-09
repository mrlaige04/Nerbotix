using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Categories;

public class CategoryBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
}