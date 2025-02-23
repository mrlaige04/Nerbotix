using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Infrastructure.Authentication.Register;

public class RegisterHandler(UserManager<User> userManager) : ICommandHandler<RegisterCommand, RegisterResponse>
{
    public async Task<ErrorOr<RegisterResponse>> Handle(
        RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.Email);
        
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded) return new RegisterResponse(user.Id, user.Email!);
        
        var errors = result.Errors
            .ToDictionary(e => e.Code, object (e) => e.Description);
            
        return Error.Failure(UserErrors.RegisterFailed, UserErrors.RegisterFailedDescription, errors);
    }
}