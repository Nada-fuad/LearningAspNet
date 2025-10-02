using Microsoft.AspNetCore.Http.HttpResults;
using M05.UrlPathVersioningMinimal.Data;

namespace M05.UrlPathVersionionMinimal.Endpoints.V1;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Versioning;
using M05.UrlPathVersioningMinimal.Responses.V1;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpointsV1(this IEndpointRouteBuilder app)
    {
        var defaultApi = app.MapGroup("/api/products");

        defaultApi.MapGet("{productId:guid}", GetProductById)
                  .WithName("GetProductByIdDefault");
        //Rout P.
        //var productApi = app.MapGroup("/api/v{version:apiVersion}/products");

        var productApi = app.MapGroup("/api/products");

        productApi.MapGet("{productId:guid}", GetProductById)
                  .WithName("GetProductByIdV1")
                  .MapToApiVersion(1.0);   

        return productApi;
    }


    private static Results<Ok<ProductResponse>, NotFound<string>> GetProductById(
        Guid productId,
        ProductRepository repository, HttpResponse response)
    {
        var product = repository.GetProductById(productId);

        if (product is null)
            return TypedResults.NotFound($"Product with Id '{productId}' not found");

        response.Headers["Deprecation"] = "true";
        response.Headers["Sunset"] = "Wed, 31 Dec 2025 23:59:59 GMT";
        response.Headers["Link"] = "</api/v2/products>; rel=\"successor-version\"";

        return TypedResults.Ok(ProductResponse.FromModel(product));
    }
}