using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiSecurity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiSecurity.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username != "student" ||
            request.Password != "12345")
        {
            return Unauthorized(new
            {
                message = "Неверный логин или пароль"
            });
        }

        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var key = _configuration["Jwt:Key"]!;
        var expiresMinutes = int.Parse(
            _configuration["Jwt:ExpiresMinutes"] ?? "1");

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Username),
            new Claim(ClaimTypes.Role, "Student")
        };

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return Ok(new
        {
            token = tokenString,
            expiresAt = expiresAt
        });
    }
}