using CalculatorAPI.Add;
using CalculatorAPI.Auth;
// using CalculatorAPI.Subtract;
// using CalculatorAPI.Multiply;
// using CalculatorAPI.Divide;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure the URLs the app should listen to in Docker, 8080 for dev, 80 when not in dev, to the docker container exposed port
if(builder.Environment.IsDevelopment())
{
builder.WebHost.UseUrls("http://0.0.0.0:8080");
}
else
{builder.WebHost.UseUrls("http://0.0.0.0:80");}

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
    };
});

// Add Authorization
builder.Services.AddAuthorization();

// Configure Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Calculator API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints for each namespace
app.MapGet("/", () => "Calculator API is running");  // Health check endpoint for debugging
app.MapJWTAuthEndpoint(builder.Configuration);
app.MapAddEndpoint();
// app.MapSubtractEndpoint();
// app.MapMultiplyEndpoint();
// app.MapDivideEndpoint();

// Enable Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
