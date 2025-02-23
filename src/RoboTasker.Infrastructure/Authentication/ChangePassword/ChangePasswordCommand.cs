using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Infrastructure.Authentication.ChangePassword;

public class ChangePasswordCommand : ICommand
{
    public string CurrentPassword { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    public string ConfirmPassword { get; set; } = null!;
}