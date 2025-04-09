using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Infrastructure.Authentication.Register;

public class RegisterCommand : ICommand
{
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Password { get; set; } = null!;
}