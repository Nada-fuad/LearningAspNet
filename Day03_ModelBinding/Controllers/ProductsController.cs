using Microsoft.AspNetCore.Mvc;

namespace Day03_ModelBinding.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
      

        [HttpGet("controller/{id}")]
        public IActionResult getProduct(int id)
        {

            return Ok($"products from controller:{id} ");
        }

        [HttpPost("body-xml-controller")]
          public IActionResult postXml(Product product)
        {
         
                return Ok(product);
            
        }
    }
}
