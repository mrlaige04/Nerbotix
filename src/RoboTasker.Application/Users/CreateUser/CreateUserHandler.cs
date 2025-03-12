using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Emails;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Common.Extensions;

namespace RoboTasker.Application.Users.CreateUser;

public class CreateUserHandler(
    UserManager<Domain.Tenants.User> userManager, IUserEmailSender emailSender) : ICommandHandler<CreateUserCommand>
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
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await emailSender.SendRegistrationEmail(user, token);
        
        return new Success();
    }
}