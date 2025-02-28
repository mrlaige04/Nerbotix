namespace RoboTasker.Application.Common.Abstractions;

public abstract class EntityResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}