using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_webAPI.Controllers.Dto;
using Store_webAPI.Data;
using Store_webAPI.Data.Entities;
using System.Linq;

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

        [HttpGet("products")]
        public ActionResult<IEnumerable<ProductDto>> Get()
        {
            var products = dbContext.Products;

            List<ProductDto> result = products.Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.UserCreatedId,
                product.TimeCreated
            )).ToList();

            return result;
        }

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

        [HttpPost("products")]
        public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto newProduct)
        {
            var product = new Product(
                Guid.NewGuid(),
                newProduct.Name,
                newProduct.Description,
                newProduct.Price,
                newProduct.UserCreatedId,
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

        [HttpPut("products/{id}")]
        public async Task<ActionResult> Update([FromRoute] Guid id,[FromBody] UpdateProductDto newProduct)
        {
            var productToUpdate = await dbContext.Products.Where(product => product.Id == id).SingleOrDefaultAsync();

            if (productToUpdate is null)
            {
                return NotFound();
            }

            productToUpdate.Id = id;
            productToUpdate.Name = newProduct.Name;
            productToUpdate.Description = newProduct.Description;
            productToUpdate.Price = newProduct.Price;
            productToUpdate.UserCreatedId = newProduct.UserCreatedId;
            productToUpdate.TimeCreated = DateTime.UtcNow;

            try
            {
                dbContext.Update(productToUpdate);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex) { return BadRequest(); }

            return NoContent();
        }

        [HttpDelete("products/{id}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            var productById = await dbContext.Products.Where(product => product.Id == id).SingleOrDefaultAsync();
            if (productById is null)
            {
                return NotFound();
            }

            dbContext.Remove(productById);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
