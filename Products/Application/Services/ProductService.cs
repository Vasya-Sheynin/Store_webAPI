using CommonModules.Domain.Entities;
using CommonModules.Domain.Interfaces;
using MediatR;
using Products.Application.Filters;
using Products.Application.ServiceInterfaces;
using Products.Application.Validation.Commands;


namespace Products.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ISender sender;

        public ProductService(IProductRepository repository, ISender s)
        {
            productRepository = repository;
            sender = s;
        }

        public async Task DeleteProductAsync(Guid id, Guid userId)
        {
            await productRepository.DeleteProductAsync(id);
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredProductsAsync(Filter filter)
        {
            await sender.Send(new ValidateFilterCommand(filter));

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
            await sender.Send(new ValidateProductInsertCommand(product));

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
            await sender.Send(new ValidateProductUpdateCommand(product));

            var productToUpdate = await productRepository.GetProductByIdAsync(id);

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.Price = product.Price;
            productToUpdate.UserCreatedId = userId;

            await productRepository.UpdateProductAsync(productToUpdate);
        }
    }
}
