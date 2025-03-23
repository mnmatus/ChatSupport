using ChatSupport.Application.Common.Interfaces;
using MediatR;

namespace ChatSupport.Application.Features.AgentQueue.Queries
{
    public class GetTeamCapacityQuery : IRequest<GetTeamCapacityVm>
    {
        internal class GetTeamCapacityQueryHandler : IRequestHandler<GetTeamCapacityQuery, GetTeamCapacityVm>
        {
            private readonly IAgentQueueService _agentQueueService;
            public GetTeamCapacityQueryHandler(IAgentQueueService agentQueueService)
            {
                _agentQueueService = agentQueueService;
            }

            public async Task<GetTeamCapacityVm> Handle(GetTeamCapacityQuery request, CancellationToken cancellationToken)
            {
                var response = new GetTeamCapacityVm();
                var teamCapacity = await _agentQueueService.GetTeamMaxQueueCapacity();
                response.TotalCapacity = teamCapacity;
                return response;
            }
        }
    }
}
