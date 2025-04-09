using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Api.Attributes;

public class OnlySuperAdminFilter(
    ICurrentUser currentUser,
    IConfiguration configuration,
    UserManager<User> userManager) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userId = currentUser.GetUserId();
        if (userId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        
        var user = await userManager.FindByIdAsync(userId.ToString()!);
        if (user == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var superAdminEmail = configuration["SuperAdmin:Email"];
        
        var hasAccess = user.Email!.Equals(superAdminEmail, StringComparison.InvariantCultureIgnoreCase);

        if (!hasAccess)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}