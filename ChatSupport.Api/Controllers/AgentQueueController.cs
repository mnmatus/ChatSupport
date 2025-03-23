using ChatSupport.Application.Features.AgentQueue.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ChatSupport.Api.Controllers
{
    public class AgentQueueController : BaseController
    {
        [HttpGet("get-team-capacity")]
        public async Task<IActionResult> GetTeamCapacity()
        {
            var request = new GetTeamCapacityQuery();
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-all-agents")]
        public async Task<IActionResult> GetAllAgents()
        {
            var request = new GetAllAgentsQuery();
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
