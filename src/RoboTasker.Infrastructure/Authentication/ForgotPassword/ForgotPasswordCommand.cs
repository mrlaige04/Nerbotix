using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Infrastructure.Authentication.ForgotPassword;

public class ForgotPasswordCommand : ICommand
{
    public string Email { get; set; } = null!;
}