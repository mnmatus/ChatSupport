using ChatSupport.Application.Common.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSupport.Application.Features.AgentQueue.Queries
{
    public class GetAllAgentsQuery : IRequest<GetAllAgentsVm>
    {
        internal class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, GetAllAgentsVm>
        {
            private readonly IAgentQueueService _agentQueueService;
            public GetAllAgentsQueryHandler(IAgentQueueService agentQueueService)
            {
                _agentQueueService = agentQueueService;
            }

            public async Task<GetAllAgentsVm> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
            {
                var response = new GetAllAgentsVm();
                var agents = await _agentQueueService.GetAllAgents();
                response.RecordCount = agents.Count();
                response.Agents = agents.Select(i => new GetAllAgentsVm.AgentDto
                {
                    Id = i.Id,
                    CanAcceptChat = i.CanAcceptChat,
                    CurrentChats = i.CurrentChats,
                    IsOnShift = i.IsOnShift,
                    MaxChats = i.MaxChats,
                    Role = i.Role.ToString()
                }).ToList();
                return response;
            }
        }
    }
}
