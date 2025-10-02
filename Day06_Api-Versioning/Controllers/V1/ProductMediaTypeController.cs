using M01.UrlPathVersioningController.Data;
using M01.UrlPathVersioningController.Responses.V1;
using Microsoft.AspNetCore.Mvc;

namespace Day06_Api_Versioning.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
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
