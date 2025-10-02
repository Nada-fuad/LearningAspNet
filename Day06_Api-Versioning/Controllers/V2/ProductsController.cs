using M01.UrlPathVersioningController.Data;
using M01.UrlPathVersioningController.Responses.V2;
using Microsoft.AspNetCore.Mvc;

namespace Day06_Api_Versioning.Controllers.V2
{
    [ApiVersion("2.0")]
    [ApiController]
    [Route("/api/products")]
    [Route("/api/v{version:apiVersion}/products")]
    public class ProductsController(ProductRepository repository) : ControllerBase
    {


        [HttpGet("{productId:Guid}")]
        public IActionResult GetProductById(Guid productId)
        {
            var product = repository.GetProductById(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(ProductResponse.FromModel(product));
        }

    }
}
