using M01.UrlPathVersioningController.Data;
using M01.UrlPathVersioningController.Responses.V2;
using Microsoft.AspNetCore.Mvc;

namespace Day06_Api_Versioning.Controllers.V2
{
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/products/media-type")]
    public class ProductMediaTypeController(ProductRepository repository) : ControllerBase
    {
        [HttpGet("{productId:guid}")]
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
