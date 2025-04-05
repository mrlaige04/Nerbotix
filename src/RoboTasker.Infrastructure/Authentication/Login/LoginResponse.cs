using RoboTasker.Application.User;

namespace RoboTasker.Infrastructure.Authentication.Login;

public class LoginResponse
{
    public AccessTokenResponse Token { get; set; } = null!;
    public CurrentUserResponse User { get; set; } = null!;
}