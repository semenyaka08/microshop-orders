namespace Catalog.DAL.Models;

public class CategorySpecification : Entity
{
    public Guid CategoryId { get; set; }
    
    public Category Category { get; set; } = null!;

    public string Name { get; set; } = null!;
    
    public string DataType { get; set; } = string.Empty; 
    
    public bool IsFilterable { get; set; } = true;
}