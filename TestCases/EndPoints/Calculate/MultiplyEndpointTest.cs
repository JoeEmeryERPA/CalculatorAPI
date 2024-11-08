using CalculatorAPI.MainProgram;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CalculatorApi.Tests
{
    public class MultiplyEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MultiplyEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task MultiplyEndpoint_ShouldReturnCorrectProduct()
        {
            // Arrange
            var a = 6;
            var b = 7;

            // Act
            var response = await _factory.CreateClient().GetAsync($"/api/calculate/multiply?a={a}&b={b}");
            response.EnsureSuccessStatusCode(); // Assert status code is 200 OK

            // Assert
            var result = await response.Content.ReadFromJsonAsync<dynamic>(); // Use ReadFromJsonAsync
            result.Result.Should().Be(a * b); // Check if the result is the product
        }
    }
}
