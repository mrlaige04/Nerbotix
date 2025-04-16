using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Application.User.CurrentUser;
using Nerbotix.Application.User.DeleteProfilePicture;
using Nerbotix.Application.User.UploadProfilePicture;
using Nerbotix.Domain.Services;

namespace Nerbotix.Api.Controllers;

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

    [HttpPut("profile-picture"), Authorize]
    public async Task<IActionResult> UpdateProfilePicture(IFormFile file)
    {
        var command = new UploadProfilePictureCommand { File = file };
        var result = await mediator.Send(command);
        return result.Match(Ok, Problem);
    }

    [HttpDelete("profile-picture"), Authorize]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var result = await mediator.Send(new DeleteProfilePictureCommand());
        return result.Match(Ok, Problem);
    }
}