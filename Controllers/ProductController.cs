using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_webAPI.Controllers.Dto;
using Store_webAPI.Data;
using Store_webAPI.Data.Entities;
using System.Linq;
using System.Security.Claims;

namespace Store_webAPI.Controllers
{
    [Route("store-api")]    
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext dbContext;

        public ProductController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpGet("products")]
        public ActionResult<IEnumerable<ProductDto>> Get(
             [FromQuery] Guid sellerId, [FromQuery] double minPrice, [FromQuery] double maxPrice = int.MaxValue)
        {
            var products = dbContext.Products;

            List<ProductDto> result = products
                .Where(product => product.Price >= minPrice && product.Price <= maxPrice && 
                    (sellerId == Guid.Empty || product.UserCreatedId == sellerId))
                .Select(product => new ProductDto(
                    product.Id,
                    product.Name,
                    product.Description,
                    product.Price,
                    product.UserCreatedId,
                    product.TimeCreated
                ))
                .ToList();


            return result;
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpGet("products/{id}")]
        public async Task<ActionResult<ProductDto>> GetById([FromRoute] Guid id)
        {
            var productToGet = await dbContext.Products.Where(product => product.Id == id).SingleOrDefaultAsync();

            if (productToGet is null)
            {
                return NotFound();
            }

            var result = new ProductDto(
                productToGet.Id,
                productToGet.Name,
                productToGet.Description,
                productToGet.Price,
                productToGet.UserCreatedId,
                productToGet.TimeCreated
            );

            return result;
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpPost("products")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto newProduct)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri)?.Value);


            var product = new Product(
                Guid.NewGuid(),
                newProduct.Name,
                newProduct.Description,
                newProduct.Price,
                userId,
                DateTime.UtcNow
            );

            try
            {
                await dbContext.AddAsync(product);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(); }

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, 
                new ProductDto(product.Id, product.Name, product.Description, product.Price, product.UserCreatedId, product.TimeCreated));
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpPut("products/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id,[FromBody] UpdateProductDto newProduct)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri)?.Value);

            var productToUpdate = await dbContext.Products.Where(product => product.Id == id).SingleOrDefaultAsync();

            if (productToUpdate is null)
            {
                return NotFound();
            }

            if (productToUpdate.UserCreatedId != userId)
            {
                return Forbid();
            }

            productToUpdate.Id = id;
            productToUpdate.Name = newProduct.Name;
            productToUpdate.Description = newProduct.Description;
            productToUpdate.Price = newProduct.Price;
            productToUpdate.UserCreatedId = userId;
            productToUpdate.TimeCreated = DateTime.UtcNow;

            try
            {
                dbContext.Update(productToUpdate);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(); }

            return NoContent();
        }

        [Authorize(Roles = "Admin, Standard")]
        [HttpDelete("products/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = Guid.Parse(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Uri)?.Value);

            var productById = await dbContext.Products.Where(product => product.Id == id).SingleOrDefaultAsync();
            if (productById is null)
            {
                return NotFound();
            }

            if (productById.UserCreatedId != userId)
            {
                return Forbid();
            }

            dbContext.Remove(productById);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
