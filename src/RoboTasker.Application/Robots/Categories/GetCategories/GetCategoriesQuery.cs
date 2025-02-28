using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;

namespace RoboTasker.Application.Robots.Categories.GetCategories;

public class GetCategoriesQuery : IQuery<PaginatedList<CategoryBaseResponse>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}