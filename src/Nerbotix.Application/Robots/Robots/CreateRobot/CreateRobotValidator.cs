using FluentValidation;

namespace Nerbotix.Application.Robots.Robots.CreateRobot;

public class CreateRobotValidator : AbstractValidator<CreateRobotCommand>
{
    public CreateRobotValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        
        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.CustomProperties)
            .Must(c =>
            {
                if (c is not { Count: > 0 }) return true;
                
                var names = c
                    .Select(i => i.Name.ToLower())
                    .Distinct();
                
                return names.Count() == c.Count;
            });
    }
}