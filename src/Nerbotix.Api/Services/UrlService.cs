using Nerbotix.Api.Controllers;
using Nerbotix.Application.Services;

namespace Nerbotix.Api.Services;

public class UrlService(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor) : IUrlService
{
    public string GetCurrentUserProfilePictureUrl(Guid userId)
    {
        var ctx = httpContextAccessor.HttpContext;
        return linkGenerator.GetUriByAction(
            httpContext: ctx!,
            action: "GetUserAvatar",
            controller: "Users",
            values: new { Id = userId });
    }
}