using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Store_Api.Controllers;
using Store_Api.Controllers.Dto;
using Store_Api.Data;
using Store_Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store_Api_Tests
{
    public class LoginControllerTest
    {
        private readonly DbContextOptions<AppDbContext> options;
        private readonly IConfiguration config;

        public LoginControllerTest()
        {
            config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Jwt:Key", "rpK3dMglgZgsosi9uFEfPbpPdukhIbiP"},
                    {"Jwt:Issuer", "https://localhost:7216/"},
                    {"Jwt:Audience", "https://localhost:7216/"}
                })
                .Build();

            options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductControllerTestDatabase")
                .Options;
        }
        public static IEnumerable<object[]> UserLoginData()
        {
            yield return new UserLoginDto[] { new UserLoginDto("John", "pass1") };
            yield return new UserLoginDto[] { new UserLoginDto("Mary", "password") };
            yield return new UserLoginDto[] { new UserLoginDto("Bob", "12345") };
        }

        public static IEnumerable<object[]> UserRegisterData()
        {
            yield return new UserRegisterDto[] { new UserRegisterDto("John", "pass1", "john@example.com") };
            yield return new UserRegisterDto[] { new UserRegisterDto("Mary", "password", "mary@example.com") };
            yield return new UserRegisterDto[] { new UserRegisterDto("Bob", "12345", "bob@example.com") };
        }

        [Theory]
        [MemberData(nameof(UserLoginData))]
        public async Task Test_Login_Fail(UserLoginDto userLoginDto)
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new LoginController(config, context);

                // Act
                var result = await controller.Login(userLoginDto);
                context.Database.EnsureDeleted();

                // Assert
                Assert.IsType<NotFoundObjectResult>(result);
            }
        }

        [Theory]
        [MemberData(nameof(UserLoginData))]
        public async Task Test_Login_Success(UserLoginDto userLoginDto)
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new LoginController(config, context);
                context.Users.AddRange(
                    new User(
                        Guid.NewGuid(),
                        userLoginDto.Name,
                        "john.doe@example.com",
                        BCrypt.Net.BCrypt.EnhancedHashPassword(userLoginDto.Password),
                        SecurityRoles.Standard
                    )
                );
                context.SaveChanges();

                // Act
                var result = await controller.Login(userLoginDto);
                context.Database.EnsureDeleted();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Theory]
        [MemberData(nameof(UserRegisterData))]
        public async Task Test_Register_Success(UserRegisterDto userRegisterDto)
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new LoginController(config, context);

                // Act
                var result = await controller.Register(userRegisterDto);
                context.Database.EnsureDeleted();

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }
        }

        [Theory]
        [MemberData(nameof(UserRegisterData))]
        public async Task Test_Register_Fails(UserRegisterDto userRegisterDto)
        {
            using (var context = new AppDbContext(options))
            {
                // Arrange
                var controller = new LoginController(config, context);
                context.Users.AddRange(
                    new User(
                        Guid.NewGuid(),
                        userRegisterDto.Name,
                        userRegisterDto.Email,
                        BCrypt.Net.BCrypt.EnhancedHashPassword(userRegisterDto.Password),
                        SecurityRoles.Standard
                    )
                );
                context.SaveChanges();

                // Act
                var result = await controller.Register(userRegisterDto);
                context.Database.EnsureDeleted();

                // Assert
                Assert.IsType<ConflictObjectResult>(result);
            }
        }
    }
}
