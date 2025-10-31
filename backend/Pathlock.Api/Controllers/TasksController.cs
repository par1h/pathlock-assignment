using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathlock.Api.DTOs;
using Pathlock.Api.Models;
using Pathlock.Api.Services;
using System;

namespace Pathlock.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly IRepository _repo;
        public TasksController(IRepository repo) { _repo = repo; }

        [HttpPost("projects/{projectId}/tasks")]
        public IActionResult Create(Guid projectId, [FromBody] CreateTaskDto dto)
        {
            var p = _repo.GetProject(projectId);
            if (p == null) return NotFound("Project not found");

            var t = new TaskItem
            {
                ProjectId = projectId,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                EstimatedHours = dto.EstimatedHours,
                Dependencies = dto.Dependencies
            };
            _repo.AddTask(t);
            return CreatedAtAction(nameof(GetTask), new { taskId = t.Id }, t);
        }

        [HttpPut("tasks/{taskId}")]
        public IActionResult Update(Guid taskId, [FromBody] UpdateTaskDto dto)
        {
            var t = _repo.GetTask(taskId);
            if (t == null) return NotFound();
            t.Title = dto.Title;
            t.Description = dto.Description;
            t.DueDate = dto.DueDate;
            t.IsCompleted = dto.IsCompleted;
            t.EstimatedHours = dto.EstimatedHours;
            _repo.UpdateTask(t);
            return Ok(t);
        }

        [AllowAnonymous]
        [HttpGet("projects/{projectId}/tasks")]
        public IActionResult GetForProject(Guid projectId)
        {
            var list = _repo.GetTasksByProject(projectId);
            return Ok(list);
        }

        [HttpDelete("tasks/{taskId}")]
        public IActionResult Delete(Guid taskId)
        {
            var t = _repo.GetTask(taskId);
            if (t == null) return NotFound();
            _repo.DeleteTask(taskId);
            return NoContent();
        }

        [HttpGet("tasks/{taskId}")]
        public IActionResult GetTask(Guid taskId)
        {
            var t = _repo.GetTask(taskId);
            if (t == null) return NotFound();
            return Ok(t);
        }
    }
}
