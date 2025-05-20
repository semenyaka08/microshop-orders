using Microsoft.AspNetCore.Mvc;
using Catalog.Grpc;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Models;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(CatalogService.CatalogServiceClient client, OrderingDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var grpcRequest = new ValidateAndGetDetailsRequest();

        grpcRequest.ItemsToValidate.AddRange(request.Items.Select(i => new ItemToValidate
        {
            ProductId = i.ProductId,
            Quantity = i.Quantity
        }));

        var catalogResponse = await client.ValidateAndGetDetailsAsync(grpcRequest);

        if (catalogResponse.OverallStatus == OverallValidationStatus.Invalid)
        {
            return BadRequest(new
            {
                Message = "One or more items are invalid",
                Errors = catalogResponse.ErrorDetails.Select(e => new
                {
                    e.ProductId,
                    e.Reason,
                    e.RequestedQuantity
                })
            });
        }

        var orderItems = catalogResponse.ValidatedItems.Select(z => new OrderItem
        {
            Price = (decimal)z.UnitPrice,
            Quantity = z.Quantity
        }).ToList();

        var order = new Order
        {
            OrderItems = orderItems
        };

        await context.Orders.AddAsync(order);

        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }
}

public record CreateOrderRequest(List<OrderItemDto> Items);

public record OrderItemDto(string ProductId, int Quantity);