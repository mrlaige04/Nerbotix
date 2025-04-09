namespace Nerbotix.Domain.Abstractions;

public abstract class BaseEntity : IEntity<Guid>, IAuditableEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}