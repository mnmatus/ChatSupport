namespace ChatSupport.Application.Features.ChatSession.Queries
{
    public class GetUnassignedChatSessionsVm
    {
        public int RecordCount { get; set; }
        public List<ChatSessionDto> ChatSessions { get; set; } = new();
        public class ChatSessionDto
        {
            public Guid Id { get; set; }
            public DateTime CreatedAt { get; set; }
        }
    }
}
