using ChatSupport.Application.Common.Interfaces;
using MediatR;

namespace ChatSupport.Application.Features.ChatSession.Commands
{
    public class ChatSessionCreated : INotification
    {
        public Guid ChatId { get; set; }

        public class ChatSessionCreatedHandler : INotificationHandler<ChatSessionCreated>
        {
            private readonly IAgentQueueService _agentQueueService;

            public ChatSessionCreatedHandler(IAgentQueueService agentQueueService)
            {
                _agentQueueService = agentQueueService;
            }

            public async Task Handle(ChatSessionCreated notification, CancellationToken cancellationToken)
            {
                //await _agentQueueService.AssignChatToAgent();
            }
        }
    }
}
