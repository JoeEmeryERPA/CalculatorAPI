namespace CalculatorAPI.Multiply
{
    public static class MultiplyEndpoint
    {
        public static void MapMultiplyEndpoint(this WebApplication app)
        {
            app.MapGet("/api/calculate/multiply", (int a, int b) => Results.Ok(new { Result = a * b }))
               .RequireAuthorization(); // Require JWT authorization for Multiply
        }
    }
}
