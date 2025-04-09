using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.Robots.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ITenantCommand;