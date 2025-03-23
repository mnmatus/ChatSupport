using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatSupport.Application.Features.AgentQueue.Queries
{
    public class GetAllAgentsVm
    {
        public int RecordCount { get; set; }

        public List<AgentDto> Agents { get; set; } = new();

        public class AgentDto
        {
            public int Id { get; set; }
            public string Role { get; set; }
            public bool IsOnShift { get; set; }
            public double MaxChats { get; set; }
            public List<Guid> CurrentChats { get; set; } = new();
            public bool CanAcceptChat { get; set; }
        }
    }
}
