using M01.UrlPathVersioningController.Data;
using M01.UrlPathVersioningController.Responses.V1;
using Microsoft.AspNetCore.Mvc;

namespace Day06_Api_Versioning.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/products")]
    [Route("/api/v{version:apiVersion}/products")]
    public class ProductsController(ProductRepository repository) : ControllerBase
    {


        [HttpGet("{productId:Guid}")]
        public IActionResult GetProductById(Guid productId)
        {

            Response.Headers["Deprecation"] = "true";
            Response.Headers["Sunset"] = "Wed, 31 Dec 2015";
            Response.Headers["Link"] = "api/v2/products ;rel=\"successor-version\"";

            var product = repository.GetProductById(productId);
            if (product == null) {
                return NotFound();
            }
            return Ok(ProductResponse.FromModel(product));
        }
         
        }
}
