﻿using Nerbotix.Application.Robots.Categories.CreateCategory;

namespace Nerbotix.Api.Models.Categories;

public class UpdateCategoryRequest
{
    public string? Name { get; set; }
    public IList<Guid>? DeletedProperties { get; set; }
    public IList<CreateCategoryCommandPropertyItem>? NewProperties { get; set; }
}