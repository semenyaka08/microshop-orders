using Catalog.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.DAL;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<Product> Products { get; set; }
    
    public DbSet<CategorySpecification> CategorySpecifications { get; set; }
    
    public DbSet<ProductSpecification> ProductSpecifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.SubCategories)
            .WithOne(c => c.ParentCategory)
            .HasForeignKey(c => c.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductSpecification>()
            .HasOne(ps => ps.Product)
            .WithMany()
            .HasForeignKey(ps => ps.ProductId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<ProductSpecification>()
            .HasOne(ps => ps.CategorySpecification)
            .WithMany()
            .HasForeignKey(ps => ps.CategorySpecificationId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}