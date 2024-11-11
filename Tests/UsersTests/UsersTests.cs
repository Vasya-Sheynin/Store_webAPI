using CommonModules.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Users.Application;
using UsersTests.Setup;

public class UserControllerTests
{
    [Fact]
    public async Task Login_ReturnsSuccess()
    {
        // Arrange
        var factory = new UsersWebAppFactory();
        var client = factory.CreateClient();
        string url = "/store-api/auth/login";
        var content = new StringContent("{\"Name\":\"user1\",\"Password\":\"password1\"}", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(url, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Register_ReturnsSuccess()
    {
        // Arrange
        var factory = new UsersWebAppFactory();
        var client = factory.CreateClient();
        string url = "/store-api/auth/signup";
        var content = new StringContent("{\"Name\":\"user3\",\"Email\":\"user3@example.com\",\"Password\":\"password3\"}", System.Text.Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(url, content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUsers_ReturnsSuccess()
    {
        // Arrange
        var factory = new UsersWebAppFactory();
        var client = factory.CreateClient();

        var admin = new User(Guid.NewGuid(), "admin", "admin@gmail.com", BCrypt.Net.BCrypt.EnhancedHashPassword("adminPassword"), SecurityRoles.Admin);
        var jwtToken = GenerateToken(admin, 1);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        string url = "/store-api/users";

        // Act
        var response = await client.GetAsync(url);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await response.Content.ReadAsStringAsync()).Count());
    }

    [Fact]
    public async Task GetUserById_ReturnsSuccess()
    {
        // Arrange
        var factory = new UsersWebAppFactory();
        var client = factory.CreateClient();

        var admin = new User(Guid.NewGuid(), "admin", "admin@gmail.com", BCrypt.Net.BCrypt.EnhancedHashPassword("adminPassword"), SecurityRoles.Admin);
        var jwtToken = GenerateToken(admin, 1);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var users = await client.GetAsync("/store-api/users");
        var user = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await users.Content.ReadAsStringAsync()).FirstOrDefault();

        string url = $"/store-api/users/{user.Id}";

        // Act
        var response = await client.GetAsync(url);
        var fetchedUser = JsonConvert.DeserializeObject<UserDto>(await response.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(user.Id, fetchedUser.Id);
        Assert.Equal(user.Name, fetchedUser.Name);
        Assert.Equal(user.Email, fetchedUser.Email);
        Assert.Equal(user.PasswordHash, fetchedUser.PasswordHash);
        Assert.Equal(user.Role, fetchedUser.Role);
    }

    [Fact]
    public async Task CreateUser_ReturnsSuccess()
    {
        // Arrange
        var factory = new UsersWebAppFactory();
        var client = factory.CreateClient();

        var admin = new User(Guid.NewGuid(), "admin", "admin@gmail.com", BCrypt.Net.BCrypt.EnhancedHashPassword("adminPassword"), SecurityRoles.Admin);
        var jwtToken = GenerateToken(admin, 1);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var user = new User(Guid.NewGuid(), "customUser", "customUser@gmail.com", BCrypt.Net.BCrypt.EnhancedHashPassword("123"), SecurityRoles.Standard);

        string url = $"/store-api/users";
        var content = new StringContent(JsonConvert.SerializeObject(new CreateUserDto
        (
            user.Name,
            user.Email,
            "123",
            user.Role
        )), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync(url, content);
        var location = response.Headers.Location;

        var fetchedUserResponse = await client.GetAsync(location);
        var fetchedUser = JsonConvert.DeserializeObject<UserDto>(await fetchedUserResponse.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal(user.Name, fetchedUser.Name);
        Assert.Equal(user.Email, fetchedUser.Email);
        Assert.Equal(user.Role, fetchedUser.Role);
    }

    [Fact]
    public async Task UpdateUser_ReturnsNoContent()
    {
        // Arrange
        var factory = new UsersWebAppFactory();
        var client = factory.CreateClient();

        var admin = new User(Guid.NewGuid(), "admin", "admin@gmail.com", BCrypt.Net.BCrypt.EnhancedHashPassword("adminPassword"), SecurityRoles.Admin);
        var jwtToken = GenerateToken(admin, 1);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var users = await client.GetAsync("/store-api/users");
        var user = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await users.Content.ReadAsStringAsync()).FirstOrDefault();
        var customUser = new User(Guid.NewGuid(), "customUser", "customUser@gmail.com", BCrypt.Net.BCrypt.EnhancedHashPassword("123"), SecurityRoles.Standard);

        var url = $"store-api/users/{user.Id}";
        var content = new StringContent(JsonConvert.SerializeObject(new UpdateUserDto
        (
            customUser.Name,
            customUser.Email,
            "123",
            customUser.Role
        )), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync(url, content);

        var fetchedUserResponse = await client.GetAsync($"store-api/users/{user.Id}");
        var fetchedUser = JsonConvert.DeserializeObject<UserDto>(await fetchedUserResponse.Content.ReadAsStringAsync());

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(customUser.Name, fetchedUser.Name);
        Assert.Equal(customUser.Email, fetchedUser.Email);
        Assert.Equal(customUser.Role, fetchedUser.Role);
    }

    [Fact]
    public async Task DeleteUser_ReturnsNoContent()
    {
        // Arrange
        var factory = new UsersWebAppFactory();
        var client = factory.CreateClient();

        var admin = new User(Guid.NewGuid(), "admin", "admin@gmail.com", BCrypt.Net.BCrypt.EnhancedHashPassword("adminPassword"), SecurityRoles.Admin);
        var jwtToken = GenerateToken(admin, 1);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

        var users = await client.GetAsync("/store-api/users");
        var userToDelete = JsonConvert.DeserializeObject<IEnumerable<UserDto>>(await users.Content.ReadAsStringAsync()).FirstOrDefault();

        string url = $"/store-api/users/{userToDelete.Id}";

        // Act
        var response = await client.DeleteAsync(url);

        var fetchedUserResponse = await client.GetAsync($"/store-api/users/{userToDelete.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, fetchedUserResponse.StatusCode);
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
