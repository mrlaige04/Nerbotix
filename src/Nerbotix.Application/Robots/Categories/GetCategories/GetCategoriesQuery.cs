using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;

namespace Nerbotix.Application.Robots.Categories.GetCategories;

public class GetCategoriesQuery : ITenantQuery<PaginatedList<CategoryBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}