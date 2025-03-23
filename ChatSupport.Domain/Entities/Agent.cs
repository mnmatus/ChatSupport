
using ChatSupport.Domain.Enums;

namespace ChatSupport.Domain.Entities
{
    public class Agent
    {
        public Agent()
        {
        }

        public Agent(Role role)
        {
            Role = role;
        }

        public int Id { get; set; }
        public Role Role { get; set; }
        public bool IsOnShift { get; set; } = true;


        public DateTime? LastSessionAssigned { get; private set; }
        public double MaxChats => (10 * Multiplier);
        public List<Guid> CurrentChats { get; private set; } = new();
        public bool CanAcceptChat => IsOnShift && CurrentChats.Count < MaxChats;
        public double Multiplier => Role switch
        {
            Role.Junior => 0.4,
            Role.MidLevel => 0.6,
            Role.Senior => 0.8,
            Role.TeamLead => 0.5,
            Role.OverflowJunior => 0.4,
            _ => 0
        };
        public void AssignChat(Guid chatId, DateTime now)
        {
            LastSessionAssigned = now;
            CurrentChats.Add(chatId);
        }
    }
}
