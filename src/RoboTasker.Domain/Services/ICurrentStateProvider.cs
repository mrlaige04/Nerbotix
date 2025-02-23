namespace RoboTasker.Domain.Services;

public interface ICurrentStateProvider
{
    Guid? GetTenantId();
    Guid? GetUserId();
}