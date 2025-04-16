using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Application.Common.Extensions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Users.DeleteUser;

public class DeleteUserHandler(
    UserManager<Domain.Tenants.User> userManager,
    IBaseRepository<Tenant> tenantRepository,
    ICurrentUser currentUser,
    IConfiguration configuration) : ICommandHandler<DeleteUserCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }
        
        var superAdminEmail = configuration["SuperAdmin:Email"];
        if (user.Email!.Equals(superAdminEmail, StringComparison.InvariantCultureIgnoreCase))
        {
            return Error.Forbidden(UserErrors.SuperAdminDelete, UserErrors.SuperAdminDeleteDescription);
        }
        
        var tenantEmail = await tenantRepository.GetWithSelectorAsync(
            t => t.Email,
            t => t.Id == currentUser.GetTenantId(),
            cancellationToken: cancellationToken);

        if (!string.IsNullOrEmpty(tenantEmail) &&
            tenantEmail.Equals(user.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            return Error.Failure(UserErrors.AdminDelete, UserErrors.AdminDeleteDescription);
        }
        
        var result = await userManager.DeleteAsync(user);
        return result.Succeeded 
            ? new Success() 
            : result.ToErrorOr<Success>(UserErrors.ChangePasswordFailed, UserErrors.ChangePasswordFailedDescription);
    }
}