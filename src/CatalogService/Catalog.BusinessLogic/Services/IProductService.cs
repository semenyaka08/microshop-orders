using Catalog.DAL.Models;

namespace Catalog.BusinessLogic.Services;

public interface IProductService
{
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken);
}