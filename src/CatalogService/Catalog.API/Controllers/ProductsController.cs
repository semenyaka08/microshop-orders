using Catalog.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProductById(Guid id, CancellationToken cancellationToken)
    {
        var product = await productService.GetProductByIdAsync(id, cancellationToken);
        
        if (product == null)
        {
            return NotFound();
        }
        
        return Ok(product);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProductsByCategoryId(Guid categoryId, CancellationToken cancellationToken)
    {
        var products = await productService.GetProductsByCategoryIdAsync(categoryId, cancellationToken);
        
        return Ok(products);
    }
}