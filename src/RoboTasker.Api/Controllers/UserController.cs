using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Domain.Services;

namespace RoboTasker.Api.Controllers;

[Route("[controller]"), Authorize]
public class UserController(ICurrentUser currentUser) : BaseController
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
}