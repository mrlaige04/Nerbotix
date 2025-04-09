namespace Nerbotix.Infrastructure.Authentication;

public class AccessTokenResponse
{
    public string AccessToken { get; set; } = null!;
    public string TokenType { get; set; } = null!;
    public string? RefreshToken { get; set; } 
}