namespace CalculatorAPI.Add
{
    public static class AddEndpoint
    {
        public static void MapAddEndpoint(this WebApplication app)
        {
            app.MapGet("/api/calculate/add", (int a, int b) => Results.Ok(new { Result = a + b }))
               .RequireAuthorization(); // Require JWT authorization for Add
        }
    }
}
