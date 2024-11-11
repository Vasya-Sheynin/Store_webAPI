using CommonModules.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Products.Application;
using ProductsTests.Setup;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace ProductsTests
{
    public class ProductsTests
    {
        [Fact]
        public async Task GetProducts_ReturnsSuccess()
        {
            // Arrange
            var factory = new ProductsWebAppFactory();
            var client = factory.CreateClient();

            var user = new User(Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), "user1", "user1@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password1"), SecurityRoles.Standard);
            var jwtToken = GenerateToken(user, 1);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            string url = "/store-api/products";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(await response.Content.ReadAsStringAsync()).Count());
        }

        [Fact]
        public async Task GetProductById_ReturnsSuccess()
        {
            // Arrange
            var factory = new ProductsWebAppFactory();
            var client = factory.CreateClient();

            var user = new User(Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), "user1", "user1@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password1"), SecurityRoles.Standard);
            var jwtToken = GenerateToken(user, 1);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var products = await client.GetAsync("/store-api/products");
            var product = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(await products.Content.ReadAsStringAsync()).FirstOrDefault();

            string url = $"/store-api/products/{product.Id}";

            // Act
            var response = await client.GetAsync(url);
            var fetchedProduct = JsonConvert.DeserializeObject<ProductDto>(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(product, fetchedProduct);
        }

        [Fact]
        public async Task CreateProduct_ReturnsSuccess()
        {
            // Arrange
            var factory = new ProductsWebAppFactory();
            var client = factory.CreateClient();

            var user = new User(Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), "user1", "user1@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password1"), SecurityRoles.Standard);
            var jwtToken = GenerateToken(user, 1);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var product = new Product(Guid.NewGuid(), "customProduct", "", 11.1, Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), DateTime.Now);

            string url = $"/store-api/products";
            var content = new StringContent(JsonConvert.SerializeObject(new CreateProductDto
            (
                product.Name,
                product.Description,
                product.Price
            )), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, content);
            var location = response.Headers.Location;

            var fetchedProductResponse = await client.GetAsync(location);
            var fetchedProduct = JsonConvert.DeserializeObject<ProductDto>(await fetchedProductResponse.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(product.Name, fetchedProduct.Name);
            Assert.Equal(product.Description, fetchedProduct.Description);
            Assert.Equal(product.UserCreatedId, fetchedProduct.UserCreatedId);
            Assert.Equal(product.Price, fetchedProduct.Price);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent()
        {
            // Arrange
            var factory = new ProductsWebAppFactory();
            var client = factory.CreateClient();

            var user = new User(Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), "user1", "user1@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password1"), SecurityRoles.Standard);
            var jwtToken = GenerateToken(user, 1);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var products = await client.GetAsync("/store-api/products");
            var product = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(await products.Content.ReadAsStringAsync()).FirstOrDefault();
            var customProduct = new ProductDto(product.Id, "customProduct", "", 3.1, Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), product.TimeCreated);

            var url = $"store-api/products/{product.Id}";
            var content = new StringContent(JsonConvert.SerializeObject(new UpdateProductDto
            (
                customProduct.Name,
                customProduct.Description,
                customProduct.Price
            )), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync(url, content);

            var fetchedProductResponse = await client.GetAsync($"store-api/products/{product.Id}");
            var fetchedProduct = JsonConvert.DeserializeObject<ProductDto>(await fetchedProductResponse.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(customProduct, fetchedProduct);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            // Arrange
            var factory = new ProductsWebAppFactory();
            var client = factory.CreateClient();

            var user = new User(Guid.Parse("c186d985-73b8-4624-96ed-30528e34de0f"), "user1", "user1@example.com", BCrypt.Net.BCrypt.EnhancedHashPassword("password1"), SecurityRoles.Standard);
            var jwtToken = GenerateToken(user, 1);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            var products = await client.GetAsync("/store-api/products");
            var productToDelete = JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(await products.Content.ReadAsStringAsync()).FirstOrDefault();

            string url = $"/store-api/products/{productToDelete.Id}";

            // Act
            var response = await client.DeleteAsync(url);

            var fetchedProductResponse = await client.GetAsync($"/store-api/products/{productToDelete.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, fetchedProductResponse.StatusCode);
        }

        private string GenerateToken(User user, int duration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("rpK3dMglgZgsosi9uFEfPbpPdukhIbiP"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Uri, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
        };

            var token = new JwtSecurityToken(
                "https://localhost:7225;http://localhost:5042",
                "https://localhost:7225;http://localhost:5042;https://localhost:7094;http://localhost:5207",
                claims,
                expires: DateTime.Now.AddMinutes(duration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}