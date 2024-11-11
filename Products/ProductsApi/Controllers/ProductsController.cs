using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application;
using Products.Application.Filters;
using Products.Application.Validation.Commands;
using Products.Application.Validation.Queries;
using System.Security.Claims;

namespace Products.ProductsApi.Controllers
{
    [Route("store-api")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ISender sender;

        public ProductsController(ISender s)
        {
            sender = s;
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get([FromQuery] Filter filter)
        {
            var products = await sender.Send(new GetProductsQuery(filter));

            return Ok(products);
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpGet("products/{id}")]
        public async Task<ActionResult<ProductDto>> GetById([FromRoute] Guid id)
        {
            var product = await sender.Send(new GetProductByIdQuery(id));

            return Ok(product);
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpPost("products")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto newProduct)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri).Value);
            var product = await sender.Send(new CreateProductCommand(newProduct, userId));

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpPut("products/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductDto newProduct)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri).Value);
            await sender.Send(new UpdateProductCommand(id, newProduct, userId));

            return NoContent();
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpDelete("products/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var userId = Guid.Parse(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri).Value);
            await sender.Send(new DeleteProductCommand(id, userId));

            return NoContent();
        }
    }
}
