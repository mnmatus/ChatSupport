namespace ChatSupport.Application.Features.ChatSession.Queries
{
    public class GetAssignedChatSessionsVm
    {
        public int RecordCount { get; set; }
        public List<ChatSessionDto> ChatSessions { get; set; } = new();
        public class ChatSessionDto
        {
            public Guid Id { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime LastPolledAt { get; set; }
            public int PollCount { get; set; }
            public bool IsActive { get; set; }
            public int? AssignedAgentId { get; set; }
        }
    }

}
