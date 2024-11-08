namespace CalculatorAPI.Subtract
{
    public static class SubtractEndpoint
    {
        public static void MapSubtractEndpoint(this WebApplication app)
        {
            app.MapGet("/api/calculate/subtract", (int a, int b) => Results.Ok(new { Result = a - b }))
               .RequireAuthorization(); // Require JWT authorization for Subtract
        }
    }
}
