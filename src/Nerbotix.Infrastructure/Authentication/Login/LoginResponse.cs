using Nerbotix.Application.User;

namespace Nerbotix.Infrastructure.Authentication.Login;

public class LoginResponse
{
    public AccessTokenResponse Token { get; set; } = null!;
    public CurrentUserResponse User { get; set; } = null!;
}