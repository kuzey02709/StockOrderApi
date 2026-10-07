using StockOrderApi.Entities;

namespace StockOrderApi.Data;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Categories.Any())
        {
            return;
        }

        var categories = new Category[]
        {
            new Category { Name = "Elektronik", Description = "Elektronik cihazlar ve aksesuarlar" },
            new Category { Name = "Ofis Malzemeleri", Description = "Masaüstü ve sarf malzemeleri" }
        };

        context.Categories.AddRange(categories);
        context.SaveChanges();

        var products = new Product[]
        {
            new Product { Name = "Kablosuz Mouse", SKU = "ELK-001", Price = 450.00m, StockQuantity = 100, CategoryId = categories[0].Id },
            new Product { Name = "Mekanik Klavye", SKU = "ELK-002", Price = 1250.00m, StockQuantity = 50, CategoryId = categories[0].Id },
            new Product { Name = "A4 Fotokopi Kağıdı", SKU = "OFS-001", Price = 120.00m, StockQuantity = 200, CategoryId = categories[1].Id }
        };

        context.Products.AddRange(products);

        var customers = new Customer[]
        {
            new Customer { CompanyName = "Yılmaz Lojistik A.Ş.", ContactName = "Ahmet Yılmaz", Email = "ahmet@yilmazlojistik.com", Phone = "05321112233" }
        };

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }
}