using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Infrastructure.Authentication.ChangePassword;
using RoboTasker.Infrastructure.Authentication.ForgotPassword;
using RoboTasker.Infrastructure.Authentication.Login;
using RoboTasker.Infrastructure.Authentication.ResetPassword;

namespace RoboTasker.Api.Controllers;

[Route("[controller]")]
public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(res => Ok(res), Problem);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(res => Ok(res), Problem);
    }

    [HttpPost("change-password"), Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(res => Ok(res), Problem);
    }
}