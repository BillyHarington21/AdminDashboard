using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminDashboard.Api.Models;

namespace AdminDashboard.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var jwtSettings = app.Configuration.GetSection("Jwt");
        var secretKey = jwtSettings["Key"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        app.MapPost("/auth/login", (LoginRequest req) =>
        {
            if (req.Email == "admin@mirra.dev" && req.Password == "admin123")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(secretKey!);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("email", req.Email),
                        new Claim(ClaimTypes.Role, "admin")
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(token);

                return Results.Ok(new { token = jwt });
            }

            return Results.Unauthorized();
        });
    }
}
