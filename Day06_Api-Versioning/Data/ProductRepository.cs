using M01.UrlPathVersioningController.Models;

namespace M01.UrlPathVersioningController.Data;

public class ProductRepository
{
    private readonly List<Product> _products =
    [
    new Product { Id = Guid.Parse("2779ee47-10b0-4bd7-8342-404006aa1392"), Name = "Keyboard", Price = 49.99m },
    ];

    public Product? GetProductById(Guid productId)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);

        return product ?? null;
    }
}