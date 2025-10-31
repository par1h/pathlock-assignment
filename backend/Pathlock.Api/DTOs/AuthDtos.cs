using System;
namespace Pathlock.Api.DTOs
{
    public record RegisterDto(string Username, string Password);
    public record LoginDto(string Username, string Password);
    public record AuthResponse(string Token);
}
