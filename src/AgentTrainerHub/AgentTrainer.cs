using System;
using System.Collections.Generic;
using AgentTrainerHub.Config;
using AgentTrainerHub.Interfaces;
using Astryx.Abstractions.Agents;
using Astryx.Abstractions.Substrate.Config;

namespace AgentTrainerHub
{
    public sealed class AgentTrainerHub : IAgentTrainerHub
    {
        private readonly IMindBuilder _mindBuilder;
        private readonly Dictionary<Guid, IAgentTrainingSession> _sessions = new();

        public AgentTrainerHub(IMindBuilder mindBuilder)
        {
            _mindBuilder = mindBuilder;
        }

        public MindConfig CreateMind(AgentTrainerConfig config)
        {
            return _mindBuilder.BuildMind(config);
        }

        public Guid CreateAgent(MindConfig mind)
        {
            var id = Guid.NewGuid();
            _sessions[id] = new Runtime.AgentTrainingSession(id);
            return id;
        }

        public AgentExportBundle StepAgent(Guid agentId, int steps)
        {
            return _sessions[agentId].Step(steps);
        }
    }
}
