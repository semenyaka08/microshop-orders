using Catalog.DAL.Models;

namespace Catalog.DAL.Seeder;

public class DataSeeder(CatalogDbContext context) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if (!context.Categories.Any() && !context.Products.Any() && !context.CategorySpecifications.Any() && !context.ProductSpecifications.Any())
        {
            var electronics = new Category { Id = Guid.NewGuid(), Name = "Electronics", Description = "Electronic devices" };
            var smartphones = new Category { Id = Guid.NewGuid(), Name = "Smartphones", Description = "Smart mobile phones", ParentCategory = electronics };
            var laptops = new Category { Id = Guid.NewGuid(), Name = "Laptops", Description = "Portable computers", ParentCategory = electronics };
            
            var homeAppliances = new Category { Id = Guid.NewGuid(), Name = "Home Appliances", Description = "Appliances for home" };
            var refrigerators = new Category { Id = Guid.NewGuid(), Name = "Refrigerators", Description = "Cooling appliances", ParentCategory = homeAppliances };
            var washingMachines = new Category { Id = Guid.NewGuid(), Name = "Washing Machines", Description = "Laundry appliances", ParentCategory = homeAppliances };

            var categories = new List<Category>
            {
                electronics,
                smartphones,
                laptops,
                homeAppliances,
                refrigerators,
                washingMachines
            };
            
            await context.Categories.AddRangeAsync(categories);
            
            var specsSmartphones = new[]
            {
                new CategorySpecification { Id = Guid.NewGuid(), Name = "RAM", DataType = "int", Category = smartphones },
                new CategorySpecification { Id = Guid.NewGuid(), Name = "Storage", DataType = "int", Category = smartphones },
                new CategorySpecification { Id = Guid.NewGuid(), Name = "Has5G", DataType = "bool", Category = smartphones }
            };
            
            var specsLaptops = new[]
            {
                new CategorySpecification { Id = Guid.NewGuid(), Name = "RAM", DataType = "int", Category = laptops },
                new CategorySpecification { Id = Guid.NewGuid(), Name = "Videocard", DataType = "string", Category = laptops },
                new CategorySpecification { Id = Guid.NewGuid(), Name = "CPU", DataType = "string", Category = laptops }
            };
            
            await context.CategorySpecifications.AddRangeAsync(specsSmartphones.Concat(specsLaptops));

            var iphone = new Product
            {
                Id = Guid.NewGuid(),
                Name = "iPhone 14",
                Quantity = 20,
                Price = 999,
                Category = smartphones,
            };

            var galaxy = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Samsung Galaxy S23",
                Quantity = 12,
                Price = 899,
                Category = smartphones,
            };

            await context.Products.AddRangeAsync(iphone, galaxy);
            
            var productSpecs = new[]
            {
                new ProductSpecification { Product = iphone, CategorySpecification = specsSmartphones[0], Value = "6" },  // RAM
                new ProductSpecification { Product = iphone, CategorySpecification = specsSmartphones[1], Value = "128" }, // Storage
                new ProductSpecification { Product = iphone, CategorySpecification = specsSmartphones[2], Value = "true" }, // Has5G

                new ProductSpecification { Product = galaxy, CategorySpecification = specsSmartphones[0], Value = "8" },
                new ProductSpecification { Product = galaxy, CategorySpecification = specsSmartphones[1], Value = "256" },
                new ProductSpecification { Product = galaxy, CategorySpecification = specsSmartphones[2], Value = "true" }
            };
            
            await context.ProductSpecifications.AddRangeAsync(productSpecs);
            await context.SaveChangesAsync();
        }
    }
}