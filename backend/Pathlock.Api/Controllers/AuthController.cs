using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pathlock.Api.DTOs;
using Pathlock.Api.Models;
using Pathlock.Api.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pathlock.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto dto)
        {
            if (_repo.GetUserByUsername(dto.Username) != null)
                return BadRequest("User already exists");

            var user = new User { Username = dto.Username, PasswordHash = dto.Password /* HASH in prod */ };
            _repo.AddUser(user);
            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            var user = _repo.GetUserByUsername(dto.Username);
            if (user == null || user.PasswordHash != dto.Password)
                return Unauthorized();

            var jwtSection = _config.GetSection("Jwt");
            var key = jwtSection["Key"]!;
            var issuer = jwtSection["Issuer"]!;
            var audience = jwtSection["Audience"]!;
            var expiryMinutes = int.Parse(jwtSection["ExpiryMinutes"] ?? "1440");

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(expiryMinutes), signingCredentials: creds);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new AuthResponse(tokenString));
        }
    }
}
