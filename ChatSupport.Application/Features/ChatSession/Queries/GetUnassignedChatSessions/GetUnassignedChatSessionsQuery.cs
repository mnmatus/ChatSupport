using ChatSupport.Application.Common.Interfaces;
using MediatR;

namespace ChatSupport.Application.Features.ChatSession.Queries
{
    public class GetUnassignedChatSessionsQuery : IRequest<GetUnassignedChatSessionsVm>
    {
        internal class GetChatSessionsQueryHandler : IRequestHandler<GetUnassignedChatSessionsQuery, GetUnassignedChatSessionsVm>
        {
            private readonly IChatSessionService _chatSessionService;
            public GetChatSessionsQueryHandler(IChatSessionService chatSessionService)
            {
                _chatSessionService = chatSessionService;
            }

            public async Task<GetUnassignedChatSessionsVm> Handle(GetUnassignedChatSessionsQuery request, CancellationToken cancellationToken)
            {
                var response = new GetUnassignedChatSessionsVm();
                var chatSessions = await _chatSessionService.GetAllUnassignedChatSessions();
                response.RecordCount = chatSessions.Count;
                response.ChatSessions = chatSessions.Select(i => new GetUnassignedChatSessionsVm.ChatSessionDto
                {
                    Id = i.Id,
                    CreatedAt = i.CreatedAt
                }).ToList();
                return response;
            }
        }
    }
}
