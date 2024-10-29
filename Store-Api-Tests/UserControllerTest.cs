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
        private readonly DbContextOptions<AppDbContext> options;

        public UserControllerTest()
        {
            options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void Test_Get_Returns_Expected_Values()
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new UserController(context);

                context.Users.AddRange(
                    new User(
                        Guid.NewGuid(), 
                        "John Doe", 
                        "john.doe@example.com", 
                        BCrypt.Net.BCrypt.EnhancedHashPassword("pass1"), 
                        SecurityRoles.Standard
                    ),
                    new User(
                        Guid.NewGuid(), 
                        "Jane Doe", 
                        "jane.doe@example.com", 
                        BCrypt.Net.BCrypt.EnhancedHashPassword("pass2"), 
                        SecurityRoles.Admin
                    )
                );
                context.SaveChanges();

                // Act
                var result = controller.Get();
                context.Database.EnsureDeleted();

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<UserDto>>>(result);
                var model = Assert.IsAssignableFrom<IEnumerable<UserDto>>(actionResult.Value);
                model = model.OrderBy(user => user.Name);
                Assert.Equal(2, model.Count());
                Assert.Equal("Jane Doe", model.ElementAt(0).Name);
                Assert.Equal("John Doe", model.ElementAt(1).Name);
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
                var controller = new UserController(context);

                context.Users.AddRange(
                    new User(
                        Guid.Parse(guid), 
                        "John Doe", 
                        "john.doe@example.com", 
                        BCrypt.Net.BCrypt.EnhancedHashPassword("pass1"), 
                        SecurityRoles.Standard
                    )
                );
                context.SaveChanges();

                // Act
                var result = await controller.GetById(Guid.Parse(guid));
                context.Database.EnsureDeleted();

                // Assert
                var actionResult = Assert.IsType<ActionResult<UserDto>>(result);
                var model = Assert.IsAssignableFrom<UserDto>(actionResult.Value);
                Assert.Equal(Guid.Parse(guid), model.Id);
                Assert.Equal("John Doe", model.Name);
                Assert.Equal("john.doe@example.com", model.Email);
                Assert.True(BCrypt.Net.BCrypt.EnhancedVerify("pass1", model.PasswordHash));
                Assert.Equal(SecurityRoles.Standard, model.Role);
            }
        }

        [Fact]
        public async Task Test_Create_Actually_Creates_Value()
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new UserController(context);
                var createUserDto = new CreateUserDto("John", "john.doe@example.com", "hash", SecurityRoles.Standard);

                // Act
                var result = await controller.Create(createUserDto);
                context.Database.EnsureDeleted();

                // Assert
                Assert.IsType<ActionResult<UserDto>>(result);
            }
        }

        [Fact]
        public async Task Test_Update_Actually_Updates_Value()
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new UserController(context);
                var userToUpdateDto = new UpdateUserDto(
                    "John Doe",
                    "john.doe@example.com",
                    "pass1",
                    SecurityRoles.Standard
                );
                var userToCreateDto = new CreateUserDto(
                    "John Doe",
                    "john.doe@example.com",
                    "pass1",
                    SecurityRoles.Standard
                );

                await controller.Create(userToCreateDto);
                var guid = controller.Get().Value.FirstOrDefault().Id;

                // Act
                var result = await controller.Update(guid, userToUpdateDto);
                var newUser = await controller.GetById(guid);
                context.Database.EnsureDeleted();

                // Assert
                Assert.IsAssignableFrom<ActionResult>(result);
                Assert.Equal(guid, newUser.Value.Id);
                Assert.Equal("John Doe", newUser.Value.Name);
                Assert.Equal("john.doe@example.com", newUser.Value.Email);
                Assert.True(BCrypt.Net.BCrypt.EnhancedVerify("pass1", newUser.Value.PasswordHash));
                Assert.Equal(SecurityRoles.Standard, newUser.Value.Role);
            }
        }

        [Fact]
        public async Task Test_Delete_Actually_Deletes_Value()
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new UserController(context);
                var userToCreateDto = new CreateUserDto(
                    "John Doe",
                    "john.doe@example.com",
                    "pass1",
                    SecurityRoles.Standard
                );

                await controller.Create(userToCreateDto);
                var guid = controller.Get().Value.FirstOrDefault().Id;

                // Act
                var result = await controller.Delete(guid);
                var countLeftUsers = controller.Get().Value.Count();
                context.Database.EnsureDeleted();

                // Assert
                Assert.IsAssignableFrom<ActionResult>(result);
                Assert.Equal(0, countLeftUsers);
            }
        }
    }
}