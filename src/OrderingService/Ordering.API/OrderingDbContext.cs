using Microsoft.EntityFrameworkCore;
using Ordering.API.Models;

namespace Ordering.API;

public class OrderingDbContext(DbContextOptions<OrderingDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    
    public DbSet<OrderItem> OrderItems { get; set; }
}