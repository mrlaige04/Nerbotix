using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Infrastructure.Authentication.Login;

public class LoginCommand : ICommand<AccessTokenResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}