using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using CommonModules.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Products.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext appDbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            appDbContext = dbContext;
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var productById = await appDbContext.Products.Where(product => product.Id == id).FirstOrDefaultAsync();
            appDbContext.Remove(productById);
            await appDbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var product = await appDbContext.Products.Where(product => product.Id == id).FirstOrDefaultAsync();

            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var products = appDbContext.Products;

            List<Product> result = await products.Select(product => new Product(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.UserCreatedId,
                product.TimeCreated
            )).ToListAsync();

            return result;
        }

        public async Task InsertProductAsync(Product product)
        {
            await appDbContext.Products.AddAsync(product);
            await appDbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            appDbContext.Products.Update(product);
            await appDbContext.SaveChangesAsync();
        }
    }
}
