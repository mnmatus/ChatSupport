using ChatSupport.Application.Common.Interfaces;
using ChatSupport.Domain.Entities;
using ChatSupport.Domain.Enums;
using ChatSupport.Infrastructure.Services;
using Moq;

namespace ChatSupport.Tests
{
    [TestClass]
    public class AgentQueueServiceTests
    {
        private Mock<IChatSessionService> _mockChatSessionService = new Mock<IChatSessionService>();
        private Mock<ISystemDateTimeService> _mockSystemDateTimeService = new Mock<ISystemDateTimeService>();
        private AgentQueueService _agentQueueService;
        private List<Agent> _agentQueue = new List<Agent> {
            new Agent { Id = 1, Role = Role.Junior },
            new Agent { Id = 2, Role = Role.MidLevel },
            new Agent { Id = 3, Role = Role.MidLevel },
        };


        [TestInitialize]
        public async Task Setup()
        {
            _mockSystemDateTimeService.Setup(s => s.Now).Returns(DateTime.UtcNow);
            _agentQueueService = new AgentQueueService(_mockSystemDateTimeService.Object, _mockChatSessionService.Object);

            ////// Seed the agent queue
            foreach (var agent in _agentQueue)
                await _agentQueueService.UpsertAgentToCapacity(agent);
        }

        [TestMethod]
        public async Task UpsertAgentToCapacity_ShouldAddNewAgent()
        {
            // Arrange
            _agentQueueService = new AgentQueueService(_mockSystemDateTimeService.Object, _mockChatSessionService.Object);

            // Seed the agent queue
            foreach (var item in _agentQueue)
                await _agentQueueService.UpsertAgentToCapacity(item);
            var agent = new Agent { Id = 4, Role = Role.Senior };

            // Act
            await _agentQueueService.UpsertAgentToCapacity(agent);

            // Assert
            var agents = await _agentQueueService.GetAllAgents();
            Assert.AreEqual(4, agents.Count);
        }

        [TestMethod]
        public async Task UpsertAgentToCapacity_ShouldUpdateExistingAgent()
        {
            // Arrange
            var agent = new Agent { Id = 1, IsOnShift = false };

            // Act
            await _agentQueueService.UpsertAgentToCapacity(agent);

            // Assert
            var agents = await _agentQueueService.GetAllAgents();
            var updatedAgent = agents.First(i => i.Id == agent.Id);
            Assert.AreEqual(false, updatedAgent.IsOnShift);
        }


        [DataTestMethod]
        [DataRow(
            16,
            1, Role.Junior, 4,
            2, Role.MidLevel, 6,
            3, Role.MidLevel, 6)]
        [DataRow(
            10,
            1, Role.Junior, 4,
            2, Role.MidLevel, 6,
            3, Role.Senior, 0)]
        [DataRow(
            9,
            1, Role.Junior, 4,
            2, Role.Junior, 4,
            3, Role.Senior, 1)]
        [DataRow(
            7,
            1, Role.Junior, 4,
            2, Role.Junior, 3,
            3, Role.OverflowJunior, 0)]
        //[DoNotParallelize]
        public async Task AssignChatToAvailableAgent_ShouldAssignChatToJuniorAgentFirst(int incomingChat,
            int agent1Id, Role agent1Role, int agent1ExpectedChatCount,
            int agent2Id, Role agent2Role, int agent2ExpectedChatCount,
            int agent3Id, Role agent3Role, int agent3ExpectedChatCount)
        {
            // Arrange
            var agents = new List<Agent>
            {
                new Agent { Id = agent1Id, Role = agent1Role },
                new Agent { Id = agent2Id, Role = agent2Role },
                new Agent { Id = agent3Id, Role = agent3Role }
            };

            _agentQueueService = new AgentQueueService(_mockSystemDateTimeService.Object, _mockChatSessionService.Object);
            _agentQueueService.ClearAgentQueue();

            foreach (var agent in agents)
                await _agentQueueService.UpsertAgentToCapacity(agent);


            for(int i = 0; i < incomingChat; i++)
            {
                // Act
                var chatId = Guid.NewGuid();
                await _agentQueueService.AssignChatToAvailableAgent(chatId);
            }

            var agentInQueue = await _agentQueueService.GetAllAgents();
            Assert.AreEqual(agent1ExpectedChatCount, agentInQueue.First(i => i.Id == agent1Id).CurrentChats.Count);
            Assert.AreEqual(agent2ExpectedChatCount, agentInQueue.First(i => i.Id == agent2Id).CurrentChats.Count);
            Assert.AreEqual(agent3ExpectedChatCount, agentInQueue.First(i => i.Id == agent3Id).CurrentChats.Count);
        }

        [TestMethod]
        public async Task IsTeamAtFullCapacity_ShouldReturnTrueWhenFull()
        {
            // Arrange

            _mockChatSessionService.Setup(s => s.GetChatSessionCreatedCount()).ReturnsAsync(24);

            // Act
            var isFull = await _agentQueueService.IsTeamAtFullCapacity();

            // Assert
            Assert.IsTrue(isFull);
        }

        [TestMethod]
        public async Task IsTeamAtFullCapacity_ShouldReturnFalseWhenNotFull()
        {
            // Arrange
            _mockChatSessionService.Setup(s => s.GetChatSessionCreatedCount()).ReturnsAsync(23);

            // Act
            var isFull = await _agentQueueService.IsTeamAtFullCapacity();

            // Assert
            Assert.IsFalse(isFull);
        }

        [TestMethod]
        public async Task IsAgentAvailableToAcceptChat_ShouldReturnTrueWhenAvailable()
        {
            // Arrange
            var maxChats = _agentQueue.Sum(i => i.CurrentChats.Count);
            for (int i = 0; i < maxChats; i++)
                await _agentQueueService.AssignChatToAvailableAgent(Guid.NewGuid());

            // Act
            var isAvailable = await _agentQueueService.IsAgentAvailableToAcceptChat();

            // Assert
            Assert.IsTrue(isAvailable);
        }
    }
}
