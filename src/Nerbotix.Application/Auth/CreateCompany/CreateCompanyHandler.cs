using ErrorOr;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nerbotix.Application.Common.Abstractions;
using Nerbotix.Application.Common.Data;
using Nerbotix.Application.Common.Emails;
using Nerbotix.Application.Common.Errors;
using Nerbotix.Domain.Consts;
using Nerbotix.Domain.Repositories.Abstractions;
using Nerbotix.Domain.Services;
using Nerbotix.Domain.Tenants;
using Nerbotix.Domain.Tenants.Settings;

namespace Nerbotix.Application.Auth.CreateCompany;

public class CreateCompanyHandler(
    IBaseRepository<Tenant> tenantRepository,
    ITenantRepository<Role> roleRepository,
    ITenantRepository<Domain.Tenants.User> userRepository,
    ICurrentUser currentUser,
    ITenantSeeder seeder,
    UserManager<Domain.Tenants.User> userManager,
    IUserEmailSender userEmailSender) : ICommandHandler<CreateCompanyCommand>
{
    public async Task<ErrorOr<Success>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        if (await tenantRepository.ExistsAsync(
                t => EF.Functions.Like(t.Name, request.Name) ||
                     EF.Functions.Like(t.Email, request.Email), 
                cancellationToken: cancellationToken))
        {
            return Error.Conflict(TenantErrors.Conflict, TenantErrors.ConflictDescription);
        }
        
        var tenantId = Guid.NewGuid();
        var tenant = new Tenant
        {
            Id = tenantId,
            Name = request.Name,
            Email = request.Email,
            Settings = TenantSettings.CreateDefault(tenantId)
        };
        
        var createdTenant = await tenantRepository.AddAsync(tenant, cancellationToken);
        
        currentUser.SetTenantId(createdTenant.Id);
        await seeder.SeedRolesAndPermissionsAsync(createdTenant.Id);
        
        var user = Domain.Tenants.User.Create(request.Email);
        user.TenantId = createdTenant.Id;
        await userManager.CreateAsync(user);

        var adminRole = await roleRepository.GetAsync(
            r => r.Name == RoleNames.Admin,
            cancellationToken: cancellationToken);
        
        user.Roles.Add(new UserRole
        {
            Role = adminRole!,
            TenantId = createdTenant.Id
        });
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        await userEmailSender.SendRegistrationEmail(user, token);
        
        return new Success();
    }
}