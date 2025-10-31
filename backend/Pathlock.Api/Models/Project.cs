using System;

namespace Pathlock.Api.Models
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid OwnerId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
