using ErrorOr;
using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Infrastructure.Authentication.ResetPassword;

public class ResetPasswordCommand : ICommand<Success>
{
    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}