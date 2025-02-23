using MediatR;
using Microsoft.AspNetCore.Mvc;
using RoboTasker.Infrastructure.Authentication.ForgotPassword;
using RoboTasker.Infrastructure.Authentication.Login;
using RoboTasker.Infrastructure.Authentication.Register;
using RoboTasker.Infrastructure.Authentication.ResetPassword;

namespace RoboTasker.Api.Controllers;

[Route("auth")]
public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(Ok, Problem);
    }

    [HttpPost("forgot")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(res => Ok(res), Problem);
    }

    [HttpPost("reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match<IActionResult>(res => Ok(res), Problem);
    }
}