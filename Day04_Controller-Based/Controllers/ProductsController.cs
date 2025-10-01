using System.Text;
using Day04_Controller_Based.Data;
using Day04_Controller_Based.Models;
using Day04_Controller_Based.Requests;
using Day04_Controller_Based.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Day04_Controller_Based.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController(ProductRepository repository) : ControllerBase
    {
        [HttpOptions]
        public IActionResult OptionsProducts()
        {
            Response.Headers.Add("Allow", "GET,POST,PUT,OPTIONS,PATCH,DELETE,HEAD");
            return NoContent();

        }

        [HttpHead("{productId:Guid}")]

        public IActionResult ExistId(Guid productId)
        {
            return repository.ExistById(productId) ? Ok() : NotFound();
        }

        [HttpGet("{productId:Guid}", Name = "GetProductById")]

        public ActionResult<ProductResponse> GetProductById(Guid productId, bool includeReviews = false)
        {
            var product = repository.GetProductById(productId);
            if (product == null)
                return NotFound();

            List<ProductReviewer> reviews = null;
            if (includeReviews = true)
            {
                reviews = repository.GetProductReviewers(productId);
            }
            return ProductResponse.FromModel(product, reviews);
        }

        [HttpGet]
        public IActionResult GetPages(int page = 1, int pageSize = 10)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            int totalCount = repository.GetProductsCount();
            var products = repository.GetProductPage(page, pageSize);

            var pagedresult = PagedResult<ProductResponse>.Create(ProductResponse.FromModels(products), totalCount, page, pageSize);

            return Ok(pagedresult);

        }


        [HttpPost]

        public IActionResult CreateProduct(CreateProductRequest request)
        {
            if (repository.ExistByName(request.Name))
            {
                return Conflict($"prodcut {request.Name} already exist");
            }
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price
            };
            repository.AddProduct(product);
            return CreatedAtRoute(routeName: nameof(GetProductById),
                routeValues: new { productId = product.Id }, value: ProductResponse.FromModel(product));
        }


        [HttpPut("{productId:Guid}")]

        public IActionResult UpdateProduct(Guid productId ,UpdateProductRequests request)
        {
            var product = repository.GetProductById(productId);
            if (product == null)
            {
                return NotFound($"product {product} not exist");
            }

            product.Name = request.Name;
            product.Price = request.Price ?? 0;

            var succeeded = repository.UpdateProduct(product);
            if (!succeeded)
            {
                return StatusCode(500, "Faild Update Product");
            }
            return NoContent();

        }

        [HttpDelete("{productId:Guid}")]

        public IActionResult DeleteProduct(Guid productId)
        {
            if (!repository.ExistById(productId))
                return NotFound(" product not found");

            var succeeded = repository.DeleteProduct(productId);

            if (!succeeded)
                return StatusCode(500, "Faild to update product");

            return NoContent();
        }


        [HttpPost("process")]
        public IActionResult Processasync()
        {
            var jobId = Guid.NewGuid();
            return Accepted($"api/products/status/{jobId}",new {jobId,status="Processing"});
        }

        [HttpGet("status/{jopId:Guid}")]
        public IActionResult GetProcessingStatus(Guid jobId)
        {
            var isStillProcessing = false;

            return Ok(new { jobId, status = isStillProcessing ? "Processing" : "Completed" });
        }

        [HttpGet("csv")]
        public IActionResult GetProductsCsv()
        {
            var products = repository.GetProductPage(1, 100);

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Id, Name, Price");

            foreach(var p in products)
            {
                csvBuilder.AppendLine($"{p.Id },{p.Name} {p.Price}");
            }
            var fileBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return File(fileBytes, "text/csv", "product-catalog-csv");

        }

        [HttpGet("physical-csv-file")]
        public IActionResult GetPhysicalFile()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),"Files","products.csv");
            return PhysicalFile(filePath, "text/csv", "products-export.csv");
        }


        [HttpGet("redirect-legacy")]
        public IActionResult GetRedirect()
        {
            return Redirect("/api/products/redirect-page");
        }

        [HttpGet("redirect-page")]
        public IActionResult GetLegacy()
        {
            return Ok(new { mesaage="you are in lagacy" });
        }

        [HttpGet("legacy-product")]
        public IActionResult RedirectCatalogProduct()
        {
            return RedirectPermanent("/api/products/products-catalog");
        }

        [HttpGet("products-catalog")]

        public IActionResult GetCatalogProduct()
        {
            return Ok(new { message = "they are your products catalog" });
        }
    }
}
