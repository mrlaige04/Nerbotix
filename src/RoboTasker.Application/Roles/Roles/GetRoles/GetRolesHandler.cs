using ErrorOr;
using RoboTasker.Application.Common.Abstractions;
using RoboTasker.Domain.Abstractions;
using RoboTasker.Domain.Repositories.Abstractions;
using RoboTasker.Domain.Tenants;

namespace RoboTasker.Application.Roles.Roles.GetRoles;

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