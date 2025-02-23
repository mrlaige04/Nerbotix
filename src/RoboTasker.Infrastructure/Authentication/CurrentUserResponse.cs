namespace RoboTasker.Infrastructure.Authentication;

public class CurrentUserResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Email { get; set; } = null!;
}