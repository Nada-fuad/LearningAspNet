using Microsoft.AspNetCore.Http.HttpResults;
using M05.UrlPathVersionionMinimal.Responses.V2;
using M05.UrlPathVersioningMinimal.Data;
using M05.UrlPathVersioningMinimal.Models;

using Asp.Versioning;




namespace M05.UrlPathVersionionMinimal.Endpoints.V2;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpointsV2(this IEndpointRouteBuilder app)
    {
        //Route Parameter
        //  var productApi = app.MapGroup("/api/v{version:apiVersion}/products");

        var productApi = app.MapGroup("/api/products");

        productApi.MapGet("{productId:guid}", GetProductById)
                  .MapToApiVersion(2.0);

        return productApi;
    }

    private static Results<Ok<ProductResponse>, NotFound<string>> GetProductById(
        Guid productId,
        ProductRepository repository)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");

        return TypedResults.Ok(ProductResponse.FromModel(product));
    }
}