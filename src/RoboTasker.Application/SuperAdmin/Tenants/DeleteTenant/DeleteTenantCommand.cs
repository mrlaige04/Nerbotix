using RoboTasker.Application.Common.Abstractions;

namespace RoboTasker.Application.SuperAdmin.Tenants.DeleteTenant;

public record DeleteTenantCommand(Guid Id) : ISuperAdminCommand;