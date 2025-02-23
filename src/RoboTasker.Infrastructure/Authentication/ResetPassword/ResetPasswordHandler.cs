using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Infrastructure.Authentication.ResetPassword;

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
            
        return Error.Failure(UserErrors.RegisterFailed, UserErrors.RegisterFailedDescription, errors);
    }
}