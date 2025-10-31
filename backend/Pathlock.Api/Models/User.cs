using System;

namespace Pathlock.Api.Models
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!; // store hashed password (demo uses plain text for simplicity)
    }
}
