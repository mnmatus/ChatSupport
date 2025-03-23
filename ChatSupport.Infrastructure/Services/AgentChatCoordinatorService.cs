using ChatSupport.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ChatSupport.Infrastructure.Services
{
    public class AgentChatCoordinatorService : BackgroundService
    {
        private readonly IChatSessionService _chatSessionService;
        private readonly IAgentQueueService _agentQueueService;
        private readonly ISystemDateTimeService _systemDateTimeService;

        public AgentChatCoordinatorService(ISystemDateTimeService systemDateTimeService, IChatSessionService chatSessionService, IAgentQueueService agentQueueService)
        {
            _chatSessionService = chatSessionService;
            _agentQueueService = agentQueueService;
            _systemDateTimeService = systemDateTimeService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = _systemDateTimeService.Now;

                var isAgentAvailable = await _agentQueueService.IsAgentAvailableToAcceptChat();
                if (isAgentAvailable)
                {
                    var chatSession = await _chatSessionService.GetNextUnassignedChatSession();
                    if (chatSession != null)
                    {
                        var agentId = await _agentQueueService.AssignChatToAvailableAgent(chatSession.Id);
                        chatSession.AssignedAgentId = agentId;
                        chatSession.IsActive = true;
                        await _chatSessionService.MoveChatSessionToAssignedList(chatSession);
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
