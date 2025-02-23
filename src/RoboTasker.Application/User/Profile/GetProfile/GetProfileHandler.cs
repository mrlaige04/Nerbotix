using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Application.Common.Errors;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Services;

namespace RoboTasker.Application.User.Profile.GetProfile;

public class GetProfileHandler(
    ICurrentUser currentUser,
    ITenantRepository<Domain.Tenants.User> userRepository) : IQueryHandler<GetProfileQuery, GetProfileResponse>
{
    public async Task<ErrorOr<GetProfileResponse>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var tenantId = currentUser.GetTenantId();
        var userId = currentUser.GetUserId();

        if (!userId.HasValue || !tenantId.HasValue)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }
        
        var profile = await userRepository.GetWithSelectorAsync(
            u => new GetProfileResponse
            {
                Email = u.Email!
            },
            u => u.Id == userId.Value,
            cancellationToken: cancellationToken);

        if (profile == null)
        {
            return Error.NotFound(UserErrors.NotFound, UserErrors.NotFoundDescription);
        }

        return profile;
    }
}