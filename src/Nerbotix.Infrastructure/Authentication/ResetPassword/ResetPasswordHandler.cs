using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Authentication.ResetPassword;

public class ResetPasswordHandler(UserManager<User> userManager) : ICommandHandler<ResetPasswordCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }
        
        var result = await userManager.ResetPasswordAsync(user, request.Code, request.Password);

        if (result.Succeeded) return new Success();
        
        var errors = result.Errors
            .ToDictionary(e => e.Code, object (e) => e.Description);
            
        return Error.Failure(UserErrors.ChangePasswordFailed, UserErrors.ChangePasswordFailedDescription, errors);
    }
}