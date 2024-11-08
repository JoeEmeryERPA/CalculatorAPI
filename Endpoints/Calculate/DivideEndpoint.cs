namespace CalculatorAPI.Divide
{
    public static class DivideEndpoint
    {
        public static void MapDivideEndpoint(this WebApplication app)
        {
            app.MapGet("/api/calculate/divide", (int a, int b) =>
            {
                if (b == 0)
                {
                    return Results.BadRequest(new { Error = "Division by zero is not allowed" });
                }
                return Results.Ok(new { Result = (double)a / b });
            })
            .RequireAuthorization(); // Require JWT authorization for Divide
        }
    }
}
