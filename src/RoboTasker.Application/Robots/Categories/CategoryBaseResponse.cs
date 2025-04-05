using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Categories;

public class CategoryBaseResponse : TenantEntityResponse
{
    public string Name { get; set; } = null!;
}