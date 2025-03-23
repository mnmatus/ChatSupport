using ChatSupport.Application.Common.Interfaces;
using MediatR;

namespace ChatSupport.Application.Features.ChatSession.Queries
{
    public class GetAssignedChatSessionsQuery : IRequest<GetAssignedChatSessionsVm>
    {
        internal class GetAssignedChatSessionsQueryHandler : IRequestHandler<GetAssignedChatSessionsQuery, GetAssignedChatSessionsVm>
        {
            private readonly IChatSessionService _chatSessionService;
            public GetAssignedChatSessionsQueryHandler(IChatSessionService chatSessionService)
            {
                _chatSessionService = chatSessionService;
            }

            public async Task<GetAssignedChatSessionsVm> Handle(GetAssignedChatSessionsQuery request, CancellationToken cancellationToken)
            {
                var response = new GetAssignedChatSessionsVm();
                var chatSessions = await _chatSessionService.GetAllAssignedChatSessions();
                response.RecordCount = chatSessions.Count;
                response.ChatSessions = chatSessions.Select(i => new GetAssignedChatSessionsVm.ChatSessionDto
                {
                    Id = i.Id,
                    CreatedAt = i.CreatedAt,
                    AssignedAgentId = i.AssignedAgentId,
                    IsActive = i.IsActive,
                    LastPolledAt = i.LastPolledAt,
                    PollCount = i.PollCount
                }).ToList();
                return response;
            }
        }
    }
}
