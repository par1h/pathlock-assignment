using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathlock.Api.DTOs;
using Pathlock.Api.Models;
using Pathlock.Api.Services;
using System;
using System.Linq;
using System.Security.Claims;

namespace Pathlock.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IRepository _repo;
        public ProjectsController(IRepository repo) { _repo = repo; }

        private Guid CurrentUserId()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userId!);
        }

        [HttpGet]
        public IActionResult Get() => Ok(_repo.GetProjectsForUser(CurrentUserId()));

        [HttpPost]
        public IActionResult Create([FromBody] CreateProjectDto dto)
        {
            var p = new Project { OwnerId = CurrentUserId(), Title = dto.Title, Description = dto.Description };
            _repo.AddProject(p);
            return CreatedAtAction(nameof(GetById), new { id = p.Id }, p);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var p = _repo.GetProject(id);
            if (p == null || p.OwnerId != CurrentUserId()) return NotFound();
            return Ok(p);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var p = _repo.GetProject(id);
            if (p == null || p.OwnerId != CurrentUserId()) return NotFound();
            _repo.DeleteProject(id);
            return NoContent();
        }
    }
}
