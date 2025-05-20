using Catalog.DAL;
using Catalog.Grpc;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Services;

public class CatalogGrpcService(CatalogDbContext dbContext) : CatalogService.CatalogServiceBase
{
    public override async Task<ValidateAndGetDetailsResponse> ValidateAndGetDetails(ValidateAndGetDetailsRequest request, ServerCallContext context)
    {
        var productIds = request.ItemsToValidate.Select(i => Guid.Parse(i.ProductId)).ToList();

        var products = await dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .AsNoTracking()
            .ToDictionaryAsync(p => p.Id);
        
        var response = new ValidateAndGetDetailsResponse();
        var allValid = true;

        foreach (var item in request.ItemsToValidate)
        {
            var productId = Guid.Parse(item.ProductId);
            if (!products.TryGetValue(productId, out var product))
            {
                allValid = false;
                response.ErrorDetails.Add(new ErrorDetail
                {
                    ProductId = item.ProductId,
                    Reason = "Product not found",
                    RequestedQuantity = item.Quantity
                });
                continue;
            }

            if (product.Quantity < item.Quantity)
            {
                allValid = false;
                response.ErrorDetails.Add(new ErrorDetail
                {
                    ProductId = item.ProductId,
                    Reason = "Not enough quantity in stock",
                    RequestedQuantity = item.Quantity
                });
                continue;
            }

            response.ValidatedItems.Add(new ValidatedItem
            {
                ProductId = product.Id.ToString(),
                ProductName = product.Name,
                UnitPrice = (double)product.Price,
                Quantity = item.Quantity
            });
        }

        response.OverallStatus = allValid
            ? OverallValidationStatus.Valid
            : OverallValidationStatus.Invalid;

        return response;
    }
}
