using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Application.Common.Extensions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Authentication.Register;

public class RegisterHandler(UserManager<User> userManager) : ICommandHandler<RegisterCommand>
{
    public async Task<ErrorOr<Success>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.Failure(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }
        
        var result = await userManager.ConfirmEmailAsync(user, request.Token);
        if (!result.Succeeded)
        {
            return result.ToErrorOr(UserErrors.ChangePasswordFailed, UserErrors.ChangePasswordFailedDescription);
        }
        
        var passwordResult = await userManager.AddPasswordAsync(user, request.Password);
        return !passwordResult.Succeeded 
            ? result.ToErrorOr(UserErrors.ChangePasswordFailed, UserErrors.ChangePasswordFailedDescription) 
            : new Success();
    }
}