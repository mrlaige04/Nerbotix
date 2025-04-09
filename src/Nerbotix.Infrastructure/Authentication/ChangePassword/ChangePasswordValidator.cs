using FluentValidation;

namespace Nerbotix.Infrastructure.Authentication.ChangePassword;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(c => c.CurrentPassword)
            .NotEmpty();
        
        RuleFor(c => c.NewPassword)
            .NotEmpty();

        RuleFor(c => c.ConfirmPassword)
            .NotEmpty()
            .Equal(c => c.NewPassword);
    }
}