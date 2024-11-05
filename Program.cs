using Microsoft.OpenApi.Models;
using CalculatorAPI.Add;
//using CalculatorAPI.Subtract;
//using CalculatorAPI.Multiply;
//using CalculatorAPI.Divide;
using CalculatorAPI.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Calculator API",
        Description = "A simple API for basic arithmetic operations"
    });
});

var app = builder.Build();

// Register each endpoint
app.MapAddEndpoint();
//app.MapSubtractEndpoint();
//app.MapMultiplyEndpoint();
//app.MapDivideEndpoint();
app.MapJWTAuthEndpoint(app.Configuration); // Pass configuration to the JWT endpoint

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculator API V1");
    options.RoutePrefix = "swagger"; // Access Swagger at `/swagger`
});

app.Run();
