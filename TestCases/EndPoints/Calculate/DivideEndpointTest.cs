using CalculatorAPI.MainProgram;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;


namespace CalculatorApi.Tests
{
    public class DivideEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public DivideEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task DivideEndpoint_ShouldReturnCorrectQuotient()
        {
            // Arrange
            var a = 20;
            var b = 5;

            // Act
            var response = await _factory.CreateClient().GetAsync($"/api/calculate/divide?a={a}&b={b}");
            response.EnsureSuccessStatusCode(); // Assert status code is 200 OK

            // Assert
            var result = await response.Content.ReadFromJsonAsync<dynamic>(); // Use ReadFromJsonAsync
            result.Result.Should().Be((double)a / b); // Check if the result is the quotient
        }

        [Fact]
        public async Task DivideEndpoint_ShouldReturnBadRequest_WhenDividingByZero()
        {
            // Arrange
            var a = 10;
            var b = 0;

            // Act
            var response = await _factory.CreateClient().GetAsync($"/api/calculate/divide?a={a}&b={b}");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest); // Check for bad request due to division by zero
            var errorResponse = await response.Content.ReadFromJsonAsync<dynamic>();
            errorResponse.Error.Should().Be("Division by zero is not allowed"); // Check if the error message matches
        }
    }
}
