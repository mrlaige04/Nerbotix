using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;

namespace Nerbotix.Application.User.Profile.GetProfile;

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