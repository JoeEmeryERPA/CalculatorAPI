using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CalculatorAPI.Auth;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using CalculatorAPI.MainProgram;

public class JWTAuthEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IConfiguration _configuration;

    // Constructor to load configuration and set up the factory
    public JWTAuthEndpointTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            // Override configuration source for tests and set base path to the correct directory
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Force loading configuration from the project root directory (not subdirectories like CalculatorAPI)
                string projectRoot = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
                config.SetBasePath(projectRoot)  // Set base path to project root
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);  // Load appsettings.json
            });
        });

        // Load configuration from appsettings.json
        string testConfigPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\..\.."));
        _configuration = new ConfigurationBuilder()
            .SetBasePath(testConfigPath)  // Make sure the correct directory is used
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    // Test case to check if the JWT token generation works correctly
    [Fact]
    public void GenerateJwtToken_ShouldReturn_ValidJwtToken()
    {
        // Arrange
        var username = "testuser";

        // Act
        var token = JWTAuthEndpoint.GenerateJwtToken(username, _configuration);  // Directly use the method

        // Assert
        Assert.NotNull(token);  // Token should not be null

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        Assert.NotNull(jwtToken);  // JWT should be readable
        Assert.Equal(_configuration["JWT:Issuer"], jwtToken.Issuer);  // Check if the issuer matches
        Assert.Contains(jwtToken.Claims, claim => claim.Type == ClaimTypes.Name && claim.Value == username);  // Username claim should be present
        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);  // Token should not be expired
    }

    // Test case for the /api/auth/token endpoint with valid credentials
    [Fact]
    public async Task TokenEndpoint_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginModel = new { Username = "testuser", Password = "testpassword" };

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/token", loginModel);

        // Assert
        response.EnsureSuccessStatusCode();  // Ensure response is successful
        var responseData = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

        Assert.True(responseData.ContainsKey("Token"));  // Response should contain "Token" key
        Assert.NotEmpty(responseData["Token"]);  // Token should not be empty
    }

    // Test case for the /api/auth/token endpoint with invalid credentials
    [Fact]
    public async Task TokenEndpoint_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
    {
        // Arrange
        var client = _factory.CreateClient();
        var loginModel = new { Username = "wronguser", Password = "wrongpassword" };

        // Act
        var response = await client.PostAsJsonAsync("/api/auth/token", loginModel);

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);  // Should return Unauthorized (401)
    }
}
