using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Infrastructure.Authentication.ForgotPassword;

public class ForgotPasswordCommand : ICommand
{
    public string Email { get; set; } = null!;
}