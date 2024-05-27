using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Models.Product;

namespace ShoppingCart.Data.Data.Product;

public static class ProductInitialize
{
    public static async void Initialize(StoreDbContext dbContext)
    {
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Products.AnyAsync())
        {
            return;
        }

        var p1 = Guid.NewGuid();
        var p2 = Guid.NewGuid();
        var p3 = Guid.NewGuid();

        var c1 = Guid.NewGuid();
        var c2 = Guid.NewGuid();

        var products = new ProductModel[]
        {
            new ProductModel { ProductID = p1, Name = "Product1", Price = 100.0f, Weight = 1.0f },
            new ProductModel { ProductID = p2, Name = "Product2", Price = 200.0f, Weight = 2.0f },
            new ProductModel { ProductID = p3, Name = "Product3", Price = 300.0f, Weight = 3.0f },
        };

        // CategoryIndex - текущее относительно массива
        // CurrentCategory - текущее относительно route

        var categories = new CategoryProductModel[]
        {
            new CategoryProductModel
            {
                CategoryID = c1, CategoryNames = new string[] { "Шоколад" }, ProductModels =
                    new HashSet<ProductModel>
                    {
                        products[0],
                        products[1]
                    }
            },

            new CategoryProductModel
            {
                CategoryID = c2, CategoryNames = new string[] { "Мясо" }, ProductModels =
                    new HashSet<ProductModel>
                    {
                        products[2]
                    }
            },
        };


        await dbContext.Products?.AddRangeAsync(products)!;
        await dbContext.CategoryProducts?.AddRangeAsync(categories)!;

        await dbContext.SaveChangesAsync();
    }
}