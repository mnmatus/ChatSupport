using ChatSupport.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSupport.Application.Features.ChatSession.Commands
{
    public class RequestChatSessionCommand : IRequest<Guid>
    {
        internal class RequestChatSessionCommandHandler : IRequestHandler<RequestChatSessionCommand, Guid>
        {
            private readonly ISystemDateTimeService _systemDateTimeService;
            private readonly IChatSessionService _chatSessionService;
            private readonly IAgentQueueService _agentQueueService;
            private readonly IMediator _mediator;

            public RequestChatSessionCommandHandler(IMediator mediator, IAgentQueueService agentQueueService, IChatSessionService chatSessionService, ISystemDateTimeService systemDateTimeService)
            {
                _systemDateTimeService = systemDateTimeService;
                _chatSessionService = chatSessionService;
                _agentQueueService = agentQueueService;
                _mediator = mediator;
            }

            public async Task<Guid> Handle(RequestChatSessionCommand request, CancellationToken cancellationToken)
            {
                var isFullCapacity = await _agentQueueService.IsTeamAtFullCapacity();

                if (isFullCapacity)
                    throw (new InvalidOperationException("Agent is at full capacity. Please try again later."));

                var id = await _chatSessionService.CreateChatSession();

                await _mediator.Publish(new ChatSessionCreated { ChatId = id }, cancellationToken);
                return id;
            }
        }
    }
}
