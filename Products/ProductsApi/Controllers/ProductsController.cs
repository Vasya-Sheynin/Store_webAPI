using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application;
using Products.Application.Filters;
using Products.Application.ServiceInterfaces;
using System.Security.Claims;

namespace Products.ProductsApi.Controllers
{
    [Route("store-api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService service)
        {
            productService = service;
        }

        [Authorize (Roles = "Admin, Standard")]
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromQuery] Filter filter)
        {
            var products = await productService.GetFilteredProductsAsync(filter);

            return Ok(products);
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpGet("products/{id}")]
        public async Task<ActionResult<ProductDto>> GetById([FromRoute] Guid id)
        {
            var product = await productService.GetProductByIdAsync(id);

            return Ok(product);
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpPost("products")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto newProduct)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri).Value);
            var product = await productService.InsertProductAsync(newProduct, userId);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpPut("products/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductDto newProduct)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri).Value);
            await productService.UpdateProductAsync(id, newProduct, userId);

            return NoContent();
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpDelete("products/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri).Value);
            await productService.DeleteProductAsync(id, userId);

            return NoContent();
        }
    }
}
