using ErrorOr;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Domain.Abstractions;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Tenants;

namespace Nerbotix.Application.Roles.Roles.GetRoles;

public class GetRolesHandler(ITenantRepository<Role> roleRepository) : IQueryHandler<GetRolesQuery, PaginatedList<RoleBaseResponse>>
{
    public async Task<ErrorOr<PaginatedList<RoleBaseResponse>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllWithSelectorPaginatedAsync(
            request.PageNumber, request.PageSize,
            r => new RoleBaseResponse
            {
                Id = r.Id,
                Name = r.Name!,
                IsSystem = r.IsSystem
            },
            cancellationToken: cancellationToken);

        return roles;
    }
}