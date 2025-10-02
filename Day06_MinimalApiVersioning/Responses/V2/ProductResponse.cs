
namespace M05.UrlPathVersionionMinimal.Responses.V2;
using M05.UrlPathVersioningMinimal.Models;

public sealed class ProductResponse
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public PriceResponse Price { get; set; } = null!;

    private ProductResponse()
    { }

    public static ProductResponse FromModel(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        var response = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = new PriceResponse
            {
                Amount = product.Price,
                Currency = "USD"
            }
        };

        return response;
    }

    public static IEnumerable<ProductResponse> FromModels(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        return products.Select(p => FromModel(p));
    }
}