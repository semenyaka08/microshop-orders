namespace Catalog.DAL.Models;

public class Category : Entity
{
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public ICollection<CategorySpecification> Specifications { get; set; } = [];
    
    public ICollection<Category > SubCategories { get; set; } = [];
    
    public Guid? ParentCategoryId { get; set; }
    
    public Category? ParentCategory { get; set; }
}