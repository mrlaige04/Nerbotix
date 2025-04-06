using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Emails;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Application.Services;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Infrastructure.Authentication.ForgotPassword;

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