using Catalog.DAL;
using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.BusinessLogic.Services;

public class ProductsService(CatalogDbContext context) : IProductService
{
    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await context.Products 
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        
        return product;
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await context.Products
            .Where(p => p.CategoryId == categoryId)
            .ToListAsync(cancellationToken); 
    }
}