using System;
using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context)
    {
        if (!context.products.Any())
        {
            var productData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/Products.json");
            var product = JsonSerializer.Deserialize<List<Product>>(productData);
            if (product == null) return;

            context.products.AddRange(product);
            await context.SaveChangesAsync();
        }
    }
}
