using CalculatorAPI.MainProgram;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CalculatorApi.Tests
{
    public class SubtractEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SubtractEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task SubtractEndpoint_ShouldReturnCorrectDifference()
        {
            // Arrange
            var a = 10;
            var b = 4;

            // Act
            var response = await _factory.CreateClient().GetAsync($"/api/calculate/subtract?a={a}&b={b}");
            response.EnsureSuccessStatusCode(); // Assert status code is 200 OK

            // Assert
            var result = await response.Content.ReadFromJsonAsync<dynamic>(); // Use ReadFromJsonAsync
            result.Result.Should().Be(a - b); // Check if the result is the difference
        }
    }
}
