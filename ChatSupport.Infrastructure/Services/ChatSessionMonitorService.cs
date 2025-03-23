using ChatSupport.Application.Common.Interfaces;
using Microsoft.Extensions.Hosting;

namespace ChatSupport.Infrastructure.Services
{
    public class ChatSessionMonitorService : BackgroundService
    {
        private readonly IChatSessionService _chatSessionService;
        private readonly IAgentQueueService _agentQueueService;
        private readonly ISystemDateTimeService _systemDateTimeService;

        public ChatSessionMonitorService(ISystemDateTimeService systemDateTimeService, IChatSessionService chatSessionService, IAgentQueueService agentQueueService)
        {
            _chatSessionService = chatSessionService;
            _systemDateTimeService = systemDateTimeService;
            _agentQueueService = agentQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = _systemDateTimeService.Now;
                var chatSessions = await _chatSessionService.GetAllAssignedChatSessions();
                foreach (var session in chatSessions)
                {
                    if ((now - session.LastPolledAt).TotalSeconds > 7)
                    {
                        await _chatSessionService.MarkSessionAsInactive(session.Id);
                        await _agentQueueService.UnAssignChatFromAgent(session.AssignedAgentId.Value, session.Id);
                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
