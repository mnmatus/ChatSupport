namespace ChatSupport.Domain.Entities
{
    public class ChatSession
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastPolledAt { get; set; }  
        public int PollCount { get; set; } = 0;
        public int? AssignedAgentId { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
