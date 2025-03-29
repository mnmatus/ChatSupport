using ChatSupport.Domain.Entities;

namespace ChatSupport.Application.Common.Interfaces
{
    public interface IAgentQueueService
    {
        Task UpsertAgentToCapacity(Agent agent);
        Task<bool> IsAgentAvailableToAcceptChat();
        Task<bool> IsTeamAtFullCapacity();
        Task<double> GetTeamMaxQueueCapacity();
        Task<int> AssignChatToAvailableAgent(Guid chatId);
        Task UnAssignChatFromAgent(int agentId, Guid chatId);
        Task UpdateAgentShift(int agentId, bool isOnShift);
        Task<List<Agent>> GetAllAgents();
        void ClearAgentQueue();
    }
}
