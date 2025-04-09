using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Categories.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : ITenantQuery<CategoryResponse>;