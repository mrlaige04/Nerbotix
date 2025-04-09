using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Api.Models.Categories;
using Nerbotix.Application.Robots.Categories.CreateCategory;
using Nerbotix.Application.Robots.Categories.DeleteCategory;
using Nerbotix.Application.Robots.Categories.GetCategories;
using Nerbotix.Application.Robots.Categories.GetCategoryById;
using Nerbotix.Application.Robots.Categories.UpdateCategory;

namespace Nerbotix.Api.Controllers;

[Route("[controller]"), Authorize]
public class CategoriesController(IMediator mediator) : BaseController
{
    [HttpPost("")]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllCategories([FromQuery] GetCategoriesQuery query)
    {
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryRequest request)
    {
        var command = request.Adapt<UpdateCategoryCommand>();
        command.Id = id;
        
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }
}