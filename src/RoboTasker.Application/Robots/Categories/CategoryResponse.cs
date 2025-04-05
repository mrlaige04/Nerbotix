namespace RoboTasker.Application.Robots.Categories;

public class CategoryResponse : CategoryBaseResponse
{
    public bool IsMaximization { get; set; }
    public IList<CategoryPropertyResponse> Properties { get; set; } = [];
}