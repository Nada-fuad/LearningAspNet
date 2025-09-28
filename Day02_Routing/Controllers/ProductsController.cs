using Microsoft.AspNetCore.Mvc;

namespace Day02_Routing.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        [HttpGet("controller")]
        public IActionResult GetProducts()
        {
            return Ok(new[]
            {
                "product 1",
                "product 2"
            });
        }

        [HttpGet("p/id")]
        public IActionResult GetProductById(int id)
        {
            return Ok($"product from controller {id}");
        }
    }
}
