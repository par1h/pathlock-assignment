using System;

namespace Pathlock.Api.DTOs
{
    public record CreateTaskDto(string Title, string? Description, DateTime? DueDate, int EstimatedHours, string[]? Dependencies);
    public record UpdateTaskDto(string Title, string? Description, DateTime? DueDate, bool IsCompleted, int EstimatedHours);
    public record SchedulerInput(TaskDto[] tasks);
    public record TaskDto(string title, int estimatedHours, string? dueDate, string[]? dependencies);
}
