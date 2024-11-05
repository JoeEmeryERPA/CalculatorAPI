using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CalculatorAPI.Auth
{
    public static class JWTAuthEndpoint
    {
        public static void MapJWTAuthEndpoint(this WebApplication app, IConfiguration configuration)
        {
            app.MapPost("/api/auth/token", (UserLoginModel login) =>
            {
                // Example validation logic
                if (login.Username == "testuser" && login.Password == "testpassword")
                {
                    var token = GenerateJwtToken(login.Username, configuration);
                    return Results.Ok(new { Token = token });
                }

                return Results.Json(new { error = "Invalid credentials." }, statusCode: 401);
;
            });
        }

        private static string GenerateJwtToken(string username, IConfiguration configuration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(configuration["JWT:ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    // Model for the login request
    public class UserLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
