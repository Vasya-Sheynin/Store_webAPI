using Products.Application.Filters;

namespace Products.Application.ServiceInterfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(Filter filter);
        Task<ProductDto> InsertProductAsync(CreateProductDto product, Guid userId);
        Task DeleteProductAsync(Guid id, Guid userId);
        Task UpdateProductAsync(Guid id, UpdateProductDto product, Guid userId);
    }
}
