using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Infrastructure.Authentication.ChangePassword;

public class ChangePasswordCommand : ICommand
{
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}