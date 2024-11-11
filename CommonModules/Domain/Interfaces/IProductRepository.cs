using CommonModules.Domain.Entities;

namespace CommonModules.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task InsertProductAsync(Product product);
        Task DeleteProductAsync(Guid id);
        Task UpdateProductAsync(Product product);
    }
}
