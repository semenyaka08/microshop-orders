namespace Catalog.DAL.Models;

public abstract class Entity
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}