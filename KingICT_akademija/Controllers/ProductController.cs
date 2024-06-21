using KingICT_akademija.Models;
using KingICT_akademija.Services;
using Microsoft.AspNetCore.Mvc;

namespace KingICT_akademija.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;

        public ProductController(ProductService _productService)
        {
            productService = _productService;
        }

        //GET api/products
        //retrieves all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await productService.GetProductsAPI();
            return Ok(products);
        }
    }
}
