using ErrorOr;
using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Infrastructure.Authentication.ResetPassword;

public class ResetPasswordCommand : ICommand<Success>
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}