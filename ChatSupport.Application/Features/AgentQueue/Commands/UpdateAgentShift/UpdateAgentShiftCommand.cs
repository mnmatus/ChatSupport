using ChatSupport.Application.Common.Interfaces;
using MediatR;

namespace ChatSupport.Application.Features.AgentQueue.Commands.UpdateAgentShift
{
    public class UpdateAgentShiftCommand : IRequest
    {
        public int Id { get; set; }
        public bool IsOnShift { get; set; }
        internal class UpdateAgentShiftCommandHandler : IRequestHandler<UpdateAgentShiftCommand>
        {
            private readonly IAgentQueueService _agentQueueService;

            public UpdateAgentShiftCommandHandler(IAgentQueueService agentQueueService)
            {
                _agentQueueService = agentQueueService;
            }

            public async Task Handle(UpdateAgentShiftCommand request, CancellationToken cancellationToken)
            {
                await _agentQueueService.UpdateAgentShift(request.Id, request.IsOnShift);
            }
        }
    }
}
