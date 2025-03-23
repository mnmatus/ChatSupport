using ChatSupport.Domain.Entities;

namespace ChatSupport.Application.Common.Interfaces
{
    public interface IChatSessionService
    {
        Task<Guid> CreateChatSession();
        Task<List<ChatSession>> GetAllUnassignedChatSessions();
        Task<List<ChatSession>> GetAllAssignedChatSessions();
        Task MoveChatSessionToAssignedList(ChatSession chatSession);
        Task<int> GetChatSessionCreatedCount();
        Task<ChatSession?> GetNextUnassignedChatSession();
        Task MarkSessionAsInactive(Guid id);

        Task<ChatSession> GetActiveSessionById(Guid id);
        Task RecordSessionPoll(Guid id);
    }
}
