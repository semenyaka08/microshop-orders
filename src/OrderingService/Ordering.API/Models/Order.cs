namespace Ordering.API.Models;

public class Order
{
    public Guid Id { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = [];
    
    public decimal TotalPrice => OrderItems.Sum(z=>z.Price * z.Quantity);
}

public class OrderItem
{
    public Guid Id { get; set; }

    public decimal Price { get; set; }
    
    public int Quantity { get; set; }
}