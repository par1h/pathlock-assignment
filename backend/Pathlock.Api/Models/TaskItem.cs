using System;

namespace Pathlock.Api.Models
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ProjectId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int EstimatedHours { get; set; } = 1;
        public string[]? Dependencies { get; set; } = Array.Empty<string>();
    }
}
