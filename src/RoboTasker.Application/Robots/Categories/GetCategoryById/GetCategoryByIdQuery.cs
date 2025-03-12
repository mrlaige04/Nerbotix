using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Categories.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : ITenantQuery<CategoryResponse>;