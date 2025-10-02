using M01.UrlPathVersioningController.Models;

namespace M01.UrlPathVersioningController.Responses.V1;

public sealed class ProductResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }

    private ProductResponse()
    { }

    public static ProductResponse FromModel(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };

        return response;
    }

    public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Select(p => FromModel(p));
    }
}