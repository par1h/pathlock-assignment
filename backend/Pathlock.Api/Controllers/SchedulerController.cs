using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pathlock.Api.DTOs;
using Pathlock.Api.Services;

namespace Pathlock.Api.Controllers
{
    [ApiController]
    [Route("api/v1/projects/{projectId}/schedule")]
    [Authorize]
    public class SchedulerController : ControllerBase
    {
        private readonly SchedulerService _scheduler;
        public SchedulerController(SchedulerService scheduler) { _scheduler = scheduler; }

        [HttpPost]
        public IActionResult Post([FromBody] SchedulerInput input)
        {
            var order = _scheduler.GetRecommendedOrder(input);
            return Ok(new { recommendedOrder = order });
        }
    }
}
