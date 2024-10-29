using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_Api.Controllers.Dto;
using Store_Api.Controllers;
using Store_Api.Data;
using Store_Api.Data.Entities;

namespace Store_Api_Tests
{
    public class ProductControllerTest
    {
        private readonly DbContextOptions<AppDbContext> options;

        public ProductControllerTest()
        {
            options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductControllerTestDatabase")
                .Options;
        }

        [Fact]
        public void Test_Get_Returns_Expected_Values()
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new ProductController(context);

                context.Products.AddRange(
                    new Product(
                        Guid.NewGuid(),
                        "Tv",
                        "",
                        40.0,
                        Guid.NewGuid(),
                        DateTime.UtcNow
                    ),
                    new Product(
                        Guid.NewGuid(),
                        "Phone",
                        "",
                        20.0,
                        Guid.NewGuid(),
                        DateTime.UtcNow
                    )
                ); 
                context.SaveChanges();

                // Act
                var result = controller.Get(Guid.Empty);
                context.Database.EnsureDeleted();

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<ProductDto>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(actionResult.Value);
                model = model.OrderBy(p => p.Name);
                Assert.Equal(2, model.Count());
                Assert.Equal("Phone", model.ElementAt(0).Name);
                Assert.Equal("Tv", model.ElementAt(1).Name);
            }
        }

        [Theory]
        [InlineData("f88400ca-4954-4920-8458-ef225d4ec03b")]
        [InlineData("29dfaf6a-e713-421a-8508-f91159b88545")]
        public async Task Test_Get_By_Id_Returns_Expected_Values(string guid)
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new ProductController(context);

                context.Products.AddRange(
                    new Product(
                        Guid.Parse(guid),
                        "Phone",
                        "",
                        20.0,
                        Guid.NewGuid(),
                        DateTime.UtcNow
                    )
                );
                context.SaveChanges();

                // Act
                var result = await controller.GetById(Guid.Parse(guid));
                context.Database.EnsureDeleted();

                // Assert
                var actionResult = Assert.IsType<ActionResult<ProductDto>>(result);
                var model = Assert.IsAssignableFrom<ProductDto>(actionResult.Value);
                Assert.Equal(Guid.Parse(guid), model.Id);
                Assert.Equal("Phone", model.Name);
                Assert.Equal(20.0, model.Price);
            }
        }
    }
}
