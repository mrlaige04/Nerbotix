using ErrorOr;
using Microsoft.AspNetCore.Identity;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Tenants;
using RoboTasker.Infrastructure.Authentication.Services;

namespace RoboTasker.Infrastructure.Authentication.Login;

public class LoginHandler(UserManager<User> userManager, TokenService tokenService) 
    : ICommandHandler<LoginCommand, AccessTokenResponse>
{
    public async Task<ErrorOr<AccessTokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Error.Unauthorized(UserErrors.InvalidPassword, UserErrors.InvalidPasswordDescription);
        }
        
        return tokenService.GenerateToken(user);
    }
}