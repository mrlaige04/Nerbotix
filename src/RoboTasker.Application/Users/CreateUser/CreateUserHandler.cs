using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Emails;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Common.Extensions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Users.CreateUser;

public class CreateUserHandler(
    UserManager<Domain.Tenants.User> userManager, IUserEmailSender emailSender,
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
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await emailSender.SendRegistrationEmail(user, token);
        
        return new Success();
    }
}