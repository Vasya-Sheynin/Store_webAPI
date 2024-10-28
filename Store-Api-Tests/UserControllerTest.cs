using Xunit;
using Store_Api.Data;
using Microsoft.EntityFrameworkCore;
using Store_Api.Data.Entities;
using Store_Api.Controllers.Dto;
using Microsoft.AspNetCore.Mvc;
using Store_Api.Controllers;

namespace Store_Api_Tests
{
    public class UserControllerTest
    {
        private readonly AppDbContext context;
        private readonly UserController controller;
        public UserControllerTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            context = new AppDbContext(options);
            controller = new UserController(context);
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            context.Users.AddRange(
                new User (Guid.NewGuid(), "John Doe", "john.doe@example.com", "hash1", SecurityRoles.Standard),
                new User (Guid.NewGuid(), "Jane Doe", "jane.doe@example.com", "hash2", SecurityRoles.Admin)
            );
            context.SaveChanges();
            
            // Act
            var result = controller.Get();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDto>>>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserDto>>(actionResult.Value);
            model = model.OrderBy(user => user.Name);
            Assert.Equal(2, model.Count());
            Assert.Equal("Jane Doe", model.ElementAt(0).Name);
            Assert.Equal("John Doe", model.ElementAt(1).Name);        
        }

    }
}