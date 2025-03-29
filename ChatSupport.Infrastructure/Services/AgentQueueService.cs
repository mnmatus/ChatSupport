using ChatSupport.Application.Common.Interfaces;
using ChatSupport.Domain.Entities;

namespace ChatSupport.Infrastructure.Services
{
    public class AgentQueueService : IAgentQueueService
    {
        private static readonly object LockObject = new();
        private static readonly List<Agent> AgentQueue = new();
        private readonly IChatSessionService _chatSessionService;
        private readonly ISystemDateTimeService _systemDateTimeService;

        public AgentQueueService(ISystemDateTimeService systemDateTimeService, IChatSessionService chatSessionService)
        {
            _chatSessionService = chatSessionService;
            _systemDateTimeService = systemDateTimeService;
        }

        public Task UpsertAgentToCapacity(Agent agent)
        {
            lock (LockObject)
            {
                var agentQueue = AgentQueue.FirstOrDefault(a => a.Id == agent.Id);
                if (agentQueue != null)
                {
                    agentQueue.IsOnShift = agent.IsOnShift;
                    agentQueue.Role = agent.Role;
                }
                else
                {
                    AgentQueue.Add(agent);
                }
                return Task.CompletedTask;
            }
        }

        public async Task<bool> IsTeamAtFullCapacity()
        {
            var teamCapacity = await GetTeamMaxQueueCapacity();
            var sessionCreated = await _chatSessionService.GetChatSessionCreatedCount();
            return (sessionCreated) >= teamCapacity;
        }

        public Task<double> GetTeamMaxQueueCapacity()
        {
            var capacity = AgentQueue.Where(i => i.Role != Domain.Enums.Role.OverflowJunior).Sum(a => a.MaxChats);
            capacity = capacity * 1.5;

            var overFlowCapacity = AgentQueue.Where(i => i.Role == Domain.Enums.Role.OverflowJunior).Sum(a => a.MaxChats);
            capacity += overFlowCapacity;
            return Task.FromResult(capacity);
        }

        public Task<int> AssignChatToAvailableAgent(Guid chatId)
        {
            lock (LockObject)
            {
                var availableAgent = AgentQueue.Where(a => a.CanAcceptChat == true)
             .OrderBy(a => a.Role).ThenBy(a => a.LastSessionAssigned)
             .FirstOrDefault();

                if (availableAgent != null)
                {
                    availableAgent.AssignChat(chatId, _systemDateTimeService.Now);
                    return Task.FromResult(availableAgent.Id);
                }

                throw new Exception("No available agent to assign the session.");
            }
        }

        public Task<List<Agent>> GetAllAgents()
        {
            var agents = AgentQueue;
            return Task.FromResult(agents);
        }

        public Task<bool> IsAgentAvailableToAcceptChat()
        {
            var availableAgent = AgentQueue.Any(i => i.CanAcceptChat == true);
            return Task.FromResult(availableAgent);
        }

        public Task UnAssignChatFromAgent(int agentId, Guid chatId)
        {
           var agent = AgentQueue.Single(a => a.Id == agentId);
           agent.CurrentChats.Remove(chatId);
           return Task.CompletedTask;
        }

        public void ClearAgentQueue()
        {
            AgentQueue.Clear();
        }

        public Task UpdateAgentShift(int agentId, bool isOnShift)
        {
            var agent = AgentQueue.Single(a => a.Id == agentId);
            agent.IsOnShift = isOnShift;
            return Task.CompletedTask;
        }
    }
}

