using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.Robots.Categories.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand;