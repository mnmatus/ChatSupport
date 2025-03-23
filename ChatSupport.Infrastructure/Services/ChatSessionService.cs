using ChatSupport.Application.Common.Interfaces;
using ChatSupport.Domain.Entities;
using System.Collections.Concurrent;

namespace ChatSupport.Infrastructure.Services
{
    public class ChatSessionService : IChatSessionService
    {
        private static readonly object LockObject = new();
        private static readonly ConcurrentQueue<ChatSession> _chatSessionUnassignedQueue = new();
        private readonly ConcurrentDictionary<Guid, ChatSession> _chatSessionAssigned = new();
        private readonly ISystemDateTimeService _systemDateTimeService;
        public ChatSessionService(ISystemDateTimeService systemDateTimeService)
        {
            _systemDateTimeService = systemDateTimeService;
        }
        public Task<Guid> CreateChatSession()
        {
            lock (LockObject)
            {
                var chatSession = new ChatSession {
                    Id = Guid.NewGuid(),
                    CreatedAt = _systemDateTimeService.Now,
                    LastPolledAt = _systemDateTimeService.Now,
                };
                _chatSessionUnassignedQueue.Enqueue(chatSession);
                return Task.FromResult(chatSession.Id);
            }
        }

        public Task<List<ChatSession>> GetAllUnassignedChatSessions()
        {
            return Task.FromResult(_chatSessionUnassignedQueue.ToList());
        }

        public Task<ChatSession?> GetNextUnassignedChatSession()
        {
            _chatSessionUnassignedQueue.TryDequeue(out var session);
            return Task.FromResult(session);
        }

        public Task MoveChatSessionToAssignedList(ChatSession chatSession)
        {
            _chatSessionAssigned.TryAdd(chatSession.Id, chatSession);
            return Task.CompletedTask;
        }

        public Task<List<ChatSession>> GetAllAssignedChatSessions()
        {
            return Task.FromResult(_chatSessionAssigned.Values.ToList());
        }

        public Task<int> GetChatSessionCreatedCount()
        {
            var assigned = _chatSessionAssigned.Where(i => i.Value.IsActive == true).Count();
            var unassigned = _chatSessionUnassignedQueue.Count;
            return Task.FromResult(assigned + unassigned);
        }

        public Task MarkSessionAsInactive(Guid id)
        {
            if (_chatSessionAssigned.TryGetValue(id, out var session))
            {
                session.IsActive = false;
                //_chatSessionAssigned.TryRemove(id, out _);
            }
            return Task.CompletedTask;
        }

        public Task RecordSessionPoll(Guid id)
        {
            if (_chatSessionAssigned.TryGetValue(id, out var session))
            {
                session.LastPolledAt = _systemDateTimeService.Now;
                session.PollCount++;
            }
            return Task.CompletedTask;
        }

        public Task<ChatSession> GetActiveSessionById(Guid id)
        {
            var session = _chatSessionAssigned[id];
            return Task.FromResult(session);
        }
    }
}
