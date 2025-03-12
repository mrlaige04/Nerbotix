using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Common.Extensions;

namespace RoboTasker.Application.Users.DeleteUser;

public class DeleteUserHandler(
    UserManager<Domain.Tenants.User> userManager) : ICommandHandler<DeleteUserCommand>
{
    public async Task<ErrorOr<Success>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());
        if (user == null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }
        
        var result = await userManager.DeleteAsync(user);
        return result.Succeeded 
            ? new Success() 
            : result.ToErrorOr<Success>(UserErrors.ChangePasswordFailed, UserErrors.ChangePasswordFailedDescription);
    }
}