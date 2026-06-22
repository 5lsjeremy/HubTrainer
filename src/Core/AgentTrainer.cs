using System;
using System.Collections.Generic;
using AgentTrainer.Core.Config;
using AgentTrainer.Core.Interfaces;
using Astryx.Abstractions.Agents;

namespace AgentTrainer.Core
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

