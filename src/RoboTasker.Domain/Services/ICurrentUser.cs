namespace RoboTasker.Domain.Services;

public interface ICurrentUser
{
    Guid? GetTenantId();
    Guid? GetUserId();
}