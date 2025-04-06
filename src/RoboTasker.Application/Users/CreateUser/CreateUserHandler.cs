using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Emails;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Common.Extensions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Users.CreateUser;

public class CreateUserHandler(
    UserManager<Domain.Tenants.User> userManager, 
    IUserEmailSender emailSender,
    ICurrentUser currentUser,
    IBaseRepository<Tenant> tenantRepository,
    RoleManager<Role> roleManager) : ICommandHandler<CreateUserCommand>
{
    public async Task<ErrorOr<Success>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(request.Email) != null)
        {
            return Error.Conflict(UserErrors.Conflict, UserErrors.ConflictDescription);
        }
        
        var user = Domain.Tenants.User.Create(request.Email);
        user.EmailConfirmed = false;

        if (!string.IsNullOrEmpty(request.Username))
        {
            user.UserName = request.Username;
        }
        
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            return result.ToErrorOr(UserErrors.FailureWhileCreating, UserErrors.FailureWhileCreatingDescription);
        }
        
        foreach (var addRole in request.Roles)
        {
            var role = await roleManager.FindByIdAsync(addRole.ToString());
            if (role != null)
            {
                user.Roles.Add(new UserRole()
                {
                    RoleId = addRole,
                });
            }
        }
        
        await userManager.UpdateAsync(user);
        
        var tenantId = currentUser.GetTenantId();
        var tenantName = await tenantRepository.GetWithSelectorAsync(
            t => t.Name,
            t => t.Id == tenantId,
            cancellationToken: cancellationToken);
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await emailSender.SendUserInvitationEmail(user, token, tenantName!);
        
        return new Success();
    }
}