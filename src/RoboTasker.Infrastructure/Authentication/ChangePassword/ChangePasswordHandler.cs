using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Infrastructure.Authentication.ChangePassword;

public class ChangePasswordHandler(ICurrentUser currentUser, UserManager<User> userManager) : ICommandHandler<ChangePasswordCommand>
{
    public async Task<ErrorOr<Success>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.GetUserId();
        if (!userId.HasValue)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        } 
        
        var user = await userManager.FindByIdAsync(userId.Value.ToString());
        if (user is null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }

        if (!await userManager.CheckPasswordAsync(user, request.CurrentPassword))
        {
            return Error.Unauthorized(UserErrors.InvalidPassword, UserErrors.InvalidPasswordDescription);
        }
        
        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded) return new Success();
        
        var errors = result.Errors
            .ToDictionary(e => e.Code, object (e) => e.Description);
            
        return Error.Failure(UserErrors.ChangePasswordFailed, UserErrors.ChangePasswordFailedDescription, errors);
    }
}