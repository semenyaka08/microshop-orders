namespace Catalog.DAL.Models;

public class Product : Entity
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
    
    public int Quantity { get; set; }
    
    public string ImageUrl { get; set; } = string.Empty;
    
    public Guid CategoryId { get; set; }
    
    public Category Category { get; set; } = null!;
}