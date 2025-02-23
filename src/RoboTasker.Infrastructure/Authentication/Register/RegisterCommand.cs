using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Infrastructure.Authentication.Register;

public class RegisterCommand : ICommand<RegisterResponse>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}