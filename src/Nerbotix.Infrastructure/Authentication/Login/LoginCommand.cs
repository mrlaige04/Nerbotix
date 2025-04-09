using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Infrastructure.Authentication.Login;

public class LoginCommand : ICommand<LoginResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}