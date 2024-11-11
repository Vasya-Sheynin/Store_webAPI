using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using Products.Application.Exceptions;
using Products.Application.Filters;
using Products.Application.ServiceInterfaces;

namespace Products.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository repository)
        {
            productRepository = repository;
        }

        public async Task DeleteProductAsync(Guid id, Guid userId)
        {
            var productById = await productRepository.GetProductByIdAsync(id);
            if (productById is null)
            {
                throw new ProductNotFoundException("ProductService");
            }

            if (productById.UserCreatedId != userId)
            {
                throw new NoAccessException("ProductService");
            }

            await productRepository.DeleteProductAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(Filter filter)
        {
            var products = (await GetProductsAsync()).Where(product =>
                    product.Price >= filter.MinPrice &&
                    product.Price <= filter.MaxPrice &&
                    (product.UserCreatedId == filter.SellerId || filter.SellerId is null)
                );

            return products;
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await productRepository.GetProductByIdAsync(id);

            if (product is null)
            {
                throw new ProductNotFoundException("ProductService");
            }

            var productDto = new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.UserCreatedId,
                product.TimeCreated
                );

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = (await productRepository.GetProductsAsync()).Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.UserCreatedId,
                product.TimeCreated
                ));

            return products;
        }

        public async Task<ProductDto> InsertProductAsync(CreateProductDto product, Guid userId)
        {
            var newProduct = new Product(
                Guid.NewGuid(),
                product.Name,
                product.Description,
                product.Price,
                userId,
                DateTime.UtcNow
                );

            await productRepository.InsertProductAsync(newProduct);

            var productDto = new ProductDto(
                newProduct.Id,
                newProduct.Name,
                newProduct.Description,
                newProduct.Price,
                newProduct.UserCreatedId,
                newProduct.TimeCreated
                );

            return productDto;
        }

        public async Task UpdateProductAsync(Guid id, UpdateProductDto product, Guid userId)
        {
            var productToUpdate = await productRepository.GetProductByIdAsync(id);

            if (productToUpdate is null)
            {
                throw new ProductNotFoundException("ProductService");
            }

            if (productToUpdate.UserCreatedId != userId)
            {
                throw new NoAccessException("ProductService");
            }

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.UserCreatedId = userId;

            await productRepository.UpdateProductAsync(productToUpdate);
        }
    }
}
