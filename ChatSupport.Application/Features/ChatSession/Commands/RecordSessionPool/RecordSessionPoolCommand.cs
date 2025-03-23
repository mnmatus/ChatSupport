using ChatSupport.Application.Common.Interfaces;
using MediatR;

namespace ChatSupport.Application.Features.ChatSession.Commands
{
    public class RecordSessionPoolCommand : IRequest
    {
        public Guid Id { get; set; }
        internal class RecordSessionPoolCommandHandler : IRequestHandler<RecordSessionPoolCommand>
        {
            private readonly IChatSessionService _chatSessionService;

            public RecordSessionPoolCommandHandler(IChatSessionService chatSessionService)
            {
                _chatSessionService = chatSessionService;
            }

            public async Task Handle(RecordSessionPoolCommand request, CancellationToken cancellationToken)
            {
                var chatSession = await _chatSessionService.GetActiveSessionById(request.Id);
                if (!chatSession.IsActive)
                {
                    throw new InvalidOperationException("Chat session is already inactive.");
                }
                await _chatSessionService.RecordSessionPoll(request.Id);
            }
        }
    }
}
