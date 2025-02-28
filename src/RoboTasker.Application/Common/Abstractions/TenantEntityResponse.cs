namespace RoboTasker.Application.Common.Abstractions;

public abstract class TenantEntityResponse : EntityResponse
{
    public Guid TenantId { get; set; }
}