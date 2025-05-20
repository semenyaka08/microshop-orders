namespace Catalog.DAL.Models;

public class ProductSpecification : Entity
{
    public Guid ProductId { get; set; }
    
    public Product Product { get; set; } = null!;

    public Guid CategorySpecificationId { get; set; }
    
    public CategorySpecification CategorySpecification { get; set; } = null!;

    public string Value { get; set; } = null!;
}