using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nerbotix.Infrastructure.Authentication.ChangePassword;
using Nerbotix.Infrastructure.Authentication.ForgotPassword;
using Nerbotix.Infrastructure.Authentication.Login;
using Nerbotix.Infrastructure.Authentication.RefreshToken;
using Nerbotix.Infrastructure.Authentication.Register;
using Nerbotix.Infrastructure.Authentication.ResetPassword;

namespace Nerbotix.Api.Controllers;

[Route("[controller]")]
public class AuthController(IMediator mediator) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match(Ok, Problem);
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match(Ok, Problem);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match(Ok, Problem);
    }

    [HttpPost("change-password"), Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match(Ok, Problem);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match(Ok, Problem);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand request)
    {
        var result = await mediator.Send(request);
        return result.Match(Ok, Problem);
    }
}