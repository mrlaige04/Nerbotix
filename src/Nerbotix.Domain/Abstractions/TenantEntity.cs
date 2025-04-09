namespace Nerbotix.Domain.Abstractions;

public abstract class TenantEntity : BaseEntity, ITenantEntity<Guid>
{
    public Guid TenantId { get; set; }
}