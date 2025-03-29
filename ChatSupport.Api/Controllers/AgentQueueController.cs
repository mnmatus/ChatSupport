using ChatSupport.Application.Features.AgentQueue.Commands.UpdateAgentShift;
using ChatSupport.Application.Features.AgentQueue.Queries;
using ChatSupport.Application.Features.ChatSession.Commands;
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

        [HttpPost("update-agent-shift")]
        public async Task<IActionResult> UpdateAgentShift([FromBody] UpdateAgentShiftCommand request)
        {
            await Mediator.Send(request);
            return Ok();
        }
    }
}
