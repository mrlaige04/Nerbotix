using Nerbotix.Application.Common.Abstractions;

namespace Nerbotix.Application.SuperAdmin.Tenants.DeleteTenant;

public record DeleteTenantCommand(Guid Id) : ISuperAdminCommand;