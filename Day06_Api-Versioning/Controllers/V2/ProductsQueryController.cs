using M01.UrlPathVersioningController.Data;
using Microsoft.AspNetCore.Mvc;
using M01.UrlPathVersioningController.Responses.V2;

namespace Day06_Api_Versioning.Controllers.V2
{

    [ApiController]
    [ApiVersion("2.0")]

    [Route("api/productsquery")]
    public class ProductsQueryController(ProductRepository repository) : Controller
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
