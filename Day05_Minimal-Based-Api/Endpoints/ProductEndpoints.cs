using System.Text;
using Day04_Controller_Based.Data;
using Day04_Controller_Based.Models;
using Day04_Controller_Based.Requests;
using Day04_Controller_Based.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
namespace Day04_Controller_Based.Endpoints
{
    
    public static class ProductEndpoints
    {


        public static RouteGroupBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
        {
            var productApi = app.MapGroup("/api/products");
            productApi.MapMethods("", ["OPTIONS"], OptionsProducts);
            productApi.MapMethods("{productId:Guid}", ["HEAD"], ExistId) ;
            productApi.MapGet("{productId:Guid}", GetProductById).WithName(nameof(GetProductById));
            productApi.MapGet("", GetPages);
            productApi.MapPost("", CreateProduct);
            productApi.MapPut("{productId:Guid}", UpdateProduct);
            productApi.MapDelete("{productId:Guid}", DeleteProduct);
            productApi.MapPost("process", Processasync);
            productApi.MapGet("status/{jopId:Guid}", GetProcessingStatus);
            productApi.MapGet("csv", GetProductsCsv);
            productApi.MapGet("physical-csv-file", GetPhysicalFile);

            return productApi;
        }
        private static IResult OptionsProducts(HttpResponse response)
        {
            response.Headers.Add("Allow", "GET,POST,PUT,OPTIONS,PATCH,DELETE,HEAD");
            return Results.NoContent();

        }


        private static IResult ExistId(Guid productId, ProductRepository repository)
        {
            return repository.ExistById(productId) ? Results.Ok() : Results.NotFound();
        }


        private static Results<Ok<ProductResponse>, NotFound<string>> GetProductById(Guid productId, ProductRepository repository, bool includeReviews = false)
        {
            var product = repository.GetProductById(productId);
            if (product == null)
                return TypedResults.NotFound("not found");

            List<ProductReviewer> reviews = null;
            if (includeReviews = true)
            {
                reviews = repository.GetProductReviewers(productId);
            }
            return TypedResults.Ok(ProductResponse.FromModel(product, reviews));
        }

        private static IResult GetPages(ProductRepository repository, int page = 1, int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            int totalCount = repository.GetProductsCount();
            var products = repository.GetProductPage(page, pageSize);

            var pagedresult = PagedResult<ProductResponse>.Create(ProductResponse.FromModels(products), totalCount, page, pageSize);

            return Results.Ok(pagedresult);

        }



        private static IResult CreateProduct(CreateProductRequest request,ProductRepository repository)
        {
            if (repository.ExistByName(request.Name))
            {
                return Results.Conflict($"prodcut {request.Name} already exist");
            }
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price
            };
            repository.AddProduct(product);
            return Results.CreatedAtRoute(routeName: nameof(GetProductById),
                routeValues: new { productId = product.Id }, value: ProductResponse.FromModel(product));
        }



        private static IResult UpdateProduct(Guid productId ,UpdateProductRequests request,ProductRepository repository)
        {
            var product = repository.GetProductById(productId);
            if (product == null)
            {
                return Results.NotFound($"product {product} not exist");
            }

            product.Name = request.Name;
            product.Price = request.Price ?? 0;

            var succeeded = repository.UpdateProduct(product);
            if (!succeeded)
            {
                return Results.StatusCode(500);
            }
            return Results.NoContent();

        }


        private static IResult DeleteProduct(Guid productId ,ProductRepository repository)
        {
            if (!repository.ExistById(productId))
                return Results.NotFound(" product not found");

            var succeeded = repository.DeleteProduct(productId);

            if (!succeeded)
                return Results.StatusCode(500);

            return Results.NoContent();
        }


        private static IResult Processasync()
        {
            var jobId = Guid.NewGuid();
            return Results.Accepted($"api/products/status/{jobId}",new {jobId,status="Processing"});
        }

        private static IResult GetProcessingStatus(Guid jobId)
        {
            var isStillProcessing = false;

            return Results.Ok(new { jobId, status = isStillProcessing ? "Processing" : "Completed" });
        }

        private static IResult GetProductsCsv(ProductRepository repository)
        {
            var products = repository.GetProductPage(1, 100);

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id, Name, Price");

            foreach(var p in products)
            {
                csvBuilder.AppendLine($"{p.Id },{p.Name} {p.Price}");
            }
            var fileBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return Results.File(fileBytes, "text/csv", "product-catalog-csv");

        }

        private static IResult GetPhysicalFile()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),"Files","products.csv");
            return TypedResults.PhysicalFile(filePath, "text/csv", "products-export.csv");
        }


       //[HttpGet("redirect-legacy")]
        private static  IResult GetRedirect()
        {
            return Results.Redirect("/api/products/redirect-page");
        }

        //[HttpGet("redirect-page")]
        private static IResult GetLegacy()
        {
            return Results.Ok(new { mesaage="you are in lagacy" });
        }

        //[HttpGet("legacy-product")]
        private static IResult RedirectCatalogProduct()
        {
            return Results.Redirect("/api/products/products-catalog", permanent:true);
        }

        //[HttpGet("products-catalog")]

        private static IResult GetCatalogProduct()
        {
            return Results.Ok(new { message = "they are your products catalog" });
        }
    }
}
