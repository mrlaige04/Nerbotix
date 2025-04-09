using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Emails;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Application.Services;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Infrastructure.Authentication.ForgotPassword;

public class ForgotPasswordHandler(
    UserManager<User> userManager, 
    IUserEmailSender userEmailSender,
    IEmailSender emailSender)
    : ICommandHandler<ForgotPasswordCommand>
{
    public async Task<ErrorOr<Success>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }
        
        var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

        await userEmailSender.SendResetPasswordEmail(user, resetToken);
        
        return new Success();
    }
}