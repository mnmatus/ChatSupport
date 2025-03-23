using ChatSupport.Application.Features.ChatSession.Commands;
using ChatSupport.Application.Features.ChatSession.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ChatSupport.Api.Controllers
{
    public class ChatSessionController : BaseController
    {
        [HttpPost("request-chat-session")]
        public async Task<IActionResult> RequestChatSession()
        {
            var request = new RequestChatSessionCommand();
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("record-session-pool")]
        public async Task<IActionResult> RecordSessionPool([FromBody] RecordSessionPoolCommand request)
        {
            await Mediator.Send(request);
            return Ok();
        }

        [HttpGet("get-all-unassigned-chat-sessions")]
        public async Task<IActionResult> GetAllUnassignedChatSessions()
        {
            var request = new GetUnassignedChatSessionsQuery();
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-all-assigned-chat-sessions")]
        public async Task<IActionResult> GetAllAssignedChatSessions()
        {
            var request = new GetAssignedChatSessionsQuery();
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
