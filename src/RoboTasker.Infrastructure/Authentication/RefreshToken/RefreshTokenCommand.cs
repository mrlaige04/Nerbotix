using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Infrastructure.Authentication.RefreshToken;

public class RefreshTokenCommand : ICommand<AccessTokenResponse>
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}