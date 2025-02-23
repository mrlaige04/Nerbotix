namespace RoboTasker.Infrastructure.Authentication.Login;

public class LoginResponse
{
    public AccessTokenResponse AccessToken { get; set; } = null!;
    public CurrentUserResponse User { get; set; } = null!;
}