using FluentValidation;

namespace Nerbotix.Application.Roles.Roles.CreateRole;

public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
    }
}