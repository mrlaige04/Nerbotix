using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Application.User.CurrentUser;
using RoboTasker.Domain.Services;

namespace RoboTasker.Api.Controllers;

[Route("[controller]"), Authorize]
public class UserController(ICurrentUser currentUser, IMediator mediator) : BaseController
{
    [HttpGet("profile"), Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = currentUser.GetUserId();
        var tenantId = currentUser.GetTenantId();
        return Ok(new
        {
            userId, tenantId
        });
    }

    [HttpGet("current-user"), Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        var result = await mediator.Send(new GetCurrentUserQuery());
        return result.Match(Ok, Problem);
    }
}