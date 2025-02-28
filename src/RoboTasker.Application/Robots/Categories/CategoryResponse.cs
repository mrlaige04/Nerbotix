namespace RoboTasker.Application.Robots.Categories;

public class CategoryResponse : CategoryBaseResponse
{
    public IList<CategoryPropertyResponse> Properties { get; set; } = [];
}