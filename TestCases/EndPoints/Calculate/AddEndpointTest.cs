using CalculatorAPI.MainProgram;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CalculatorApi.Tests
{
    public class AddEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AddEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddEndpoint_ShouldReturnCorrectSum()
        {
            // Arrange
            var a = 5;
            var b = 3;

            // Act
            var response = await _factory.CreateClient().GetAsync($"/api/calculate/add?a={a}&b={b}");
            response.EnsureSuccessStatusCode(); // Assert status code is 200 OK

            // Assert
            var result = await response.Content.ReadFromJsonAsync<dynamic>(); // Use ReadFromJsonAsync
            result.Result.Should().Be(a + b); // Check if the result is the sum
        }
    }
}
