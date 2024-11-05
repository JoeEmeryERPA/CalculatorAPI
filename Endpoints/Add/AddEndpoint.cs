using Microsoft.AspNetCore.Builder;

namespace CalculatorAPI.Add
{
    public static class AddEndpoint
    {
        public static void MapAddEndpoint(this WebApplication app)
        {
            app.MapGet("/api/calculate/add", (double num1, double num2) =>
            {
                return Results.Ok(new { Operation = "Addition", Result = num1 + num2 });
            });
        }
    }
}