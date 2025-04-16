namespace Nerbotix.Application.Services;

public interface IUrlService
{
    string GetCurrentUserProfilePictureUrl(Guid userId);
}